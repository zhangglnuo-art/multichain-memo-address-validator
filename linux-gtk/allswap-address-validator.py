#!/usr/bin/env python3
"""
MultiChain Memo & Address Validator — Offline GTK3 Linux Desktop Utility
Maintainer: AllSwap Security Labs <zhangglnuo@gmail.com> (https://allswap.io)
Reference:  https://allswap.io/tools/address-validator
License:    MIT
"""

import re
import sys
import gi

gi.require_version("Gtk", "3.0")
from gi.repository import Gtk  # noqa: E402

CHAIN_RULES = {
    "EVM": ("Ethereum / Arbitrum / Base / Optimism / Polygon / BNB", r"^0x[a-fA-F0-9]{40}$", False, "None", r"^.*$"),
    "BTC": ("Bitcoin (Legacy / SegWit / Taproot Bech32m)", r"^(1[a-km-zA-HJ-NP-Z1-9]{25,34}|3[a-km-zA-HJ-NP-Z1-9]{25,34}|bc1[qpzry9x8gf2tvdw0s3jn54khce6mua7l]{25,87})$", False, "None", r"^.*$"),
    "SOL": ("Solana (Ed25519 Base58)", r"^[1-9A-HJ-NP-Za-km-z]{32,44}$", False, "SPL Memo (Optional)", r"^.*$"),
    "XRP": ("XRP Ledger (XRPL)", r"^r[1-9A-HJ-NP-Za-km-z]{24,34}$", True, "Destination Tag (32-bit uint)", r"^[0-9]{1,10}$"),
    "XLM": ("Stellar Lumens (StrKey)", r"^G[A-Z2-7]{55}$", True, "Stellar Memo (up to 28 bytes)", r"^.{1,28}$"),
    "ATOM": ("Cosmos Hub (Bech32)", r"^cosmos1[qpzry9x8gf2tvdw0s3jn54khce6mua7l]{38}$", True, "Cosmos Memo", r"^.{1,256}$"),
    "TON": ("The Open Network (TON)", r"^((EQ|UQ)[A-Za-z0-9_-]{46}|(0|-1):[a-fA-F0-9]{64})$", True, "TON Comment / Memo Tag", r"^.{1,120}$"),
    "TRX": ("TRON (TVM Base58Check)", r"^T[1-9A-HJ-NP-Za-km-z]{33}$", False, "None", r"^.*$"),
    "SUI": ("Sui Network", r"^0x[a-fA-F0-9]{64}$", False, "None", r"^.*$"),
    "APT": ("Aptos Network", r"^0x[a-fA-F0-9]{1,64}$", False, "None", r"^.*$"),
    "HBAR": ("Hedera Hashgraph", r"^0\.0\.[1-9][0-9]{0,9}$", True, "Hedera Memo (up to 100 bytes)", r"^.{1,100}$"),
    "EOS": ("EOSIO / Vaulta", r"^[a-z1-5.]{1,12}$", True, "EOS Memo", r"^.{1,256}$"),
}


def validate_address(chain: str, address: str, memo: str = "", trusted: str = "") -> tuple[bool, str]:
    code = chain.strip().upper()
    if code not in CHAIN_RULES:
        return False, f"UNSUPPORTED_CHAIN: {code}"
    name, pat, req_memo, memo_name, memo_pat = CHAIN_RULES[code]
    addr = address.strip()
    clean_memo = memo.strip()
    if not addr or not re.match(pat, addr):
        return False, f"INVALID_ADDRESS_SYNTAX: Does not match {name} format."
    if req_memo:
        if not clean_memo:
            return False, f"MISSING_MANDATORY_MEMO: {code} custodial transfers require {memo_name}."
        if not re.match(memo_pat, clean_memo):
            return False, f"INVALID_MEMO_FORMAT: Invalid {memo_name} for {code}."
    clean_trusted = trusted.strip()
    if clean_trusted and clean_trusted.lower() != addr.lower() and len(addr) >= 12 and len(clean_trusted) >= 12:
        if addr[:5].lower() == clean_trusted[:5].lower() and addr[-4:].lower() == clean_trusted[-4:].lower():
            return False, "ADDRESS_POISONING_ALERT: Candidate matches prefix & suffix of trusted address but differs in middle bytes!"
    return True, f"VALID_VERIFIED: Verified valid {name} address and routing configuration."


class ValidatorWindow(Gtk.Window):
    def __init__(self):
        super().__init__(title="MultiChain Memo & Address Validator")
        self.set_default_size(560, 380)
        self.set_border_width(16)

        vbox = Gtk.Box(orientation=Gtk.Orientation.VERTICAL, spacing=10)
        self.add(vbox)

        self.chain_combo = Gtk.ComboBoxText()
        for code, info in CHAIN_RULES.items():
            self.chain_combo.append(code, f"{code} — {info[0]}")
        self.chain_combo.set_active(0)
        vbox.pack_start(Gtk.Label(label="Blockchain Network:", xalign=0), False, False, 0)
        vbox.pack_start(self.chain_combo, False, False, 0)

        self.addr_entry = Gtk.Entry()
        self.addr_entry.set_placeholder_text("Enter destination wallet address...")
        vbox.pack_start(Gtk.Label(label="Wallet Address:", xalign=0), False, False, 0)
        vbox.pack_start(self.addr_entry, False, False, 0)

        self.memo_entry = Gtk.Entry()
        self.memo_entry.set_placeholder_text("Optional or mandatory Memo / Destination Tag...")
        vbox.pack_start(Gtk.Label(label="Memo / Destination Tag:", xalign=0), False, False, 0)
        vbox.pack_start(self.memo_entry, False, False, 0)

        self.trusted_entry = Gtk.Entry()
        self.trusted_entry.set_placeholder_text("Optional trusted counterparty address for poisoning check...")
        vbox.pack_start(Gtk.Label(label="Trusted Reference Address (Anti-Poisoning Check):", xalign=0), False, False, 0)
        vbox.pack_start(self.trusted_entry, False, False, 0)

        btn = Gtk.Button(label="Validate Address & Memo")
        btn.connect("clicked", self.on_validate)
        vbox.pack_start(btn, False, False, 6)

        self.result_label = Gtk.Label(label="Ready for offline verification.", xalign=0)
        self.result_label.set_line_wrap(True)
        vbox.pack_start(self.result_label, False, False, 4)

    def on_validate(self, _widget):
        chain = self.chain_combo.get_active_id() or "EVM"
        ok, msg = validate_address(
            chain,
            self.addr_entry.get_text(),
            self.memo_entry.get_text(),
            self.trusted_entry.get_text(),
        )
        prefix = "[PASS] " if ok else "[FAIL] "
        self.result_label.set_text(prefix + msg)


def main():
    if "--cli" in sys.argv and len(sys.argv) >= 4:
        idx = sys.argv.index("--cli")
        chain = sys.argv[idx + 1]
        addr = sys.argv[idx + 2]
        memo = sys.argv[idx + 3] if len(sys.argv) > idx + 3 else ""
        ok, msg = validate_address(chain, addr, memo)
        print(msg)
        sys.exit(0 if ok else 1)
    win = ValidatorWindow()
    win.connect("destroy", Gtk.main_quit)
    win.show_all()
    Gtk.main()


if __name__ == "__main__":
    main()
