# MultiChain Memo & Address Validator (`multichain-memo-address-validator`)

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![Platform: Windows / Cross-Platform CLI](https://img.shields.io/badge/Platform-Windows%20%7C%20CLI%20%7C%20Node.js-blue.svg)](https://allswap.io/tools/address-validator)

A free, zero-dependency, MIT-licensed desktop & CLI utility and library for validating cryptocurrency wallet addresses, checksum formats (EIP-55, Bech32/Bech32m, Base58Check, SS58), and mandatory **Memo / Destination Tag / Payment ID** rules across **46+ blockchains**.

- **Official Web Tool & Documentation:** [https://allswap.io/tools/address-validator](https://allswap.io/tools/address-validator)
- **Project Homepage:** [https://allswap.io](https://allswap.io)
- **API & Developer Docs:** [https://allswap.io/docs](https://allswap.io/docs)

---

## Key Features

1. **46+ Blockchain Address Format & Checksum Verification**
   - **Bitcoin & UTXO Chains:** Legacy (`P2PKH`), Script (`P2SH`), Native SegWit (`Bech32`), Taproot (`Bech32m`) for BTC, LTC, DOGE, BCH, DASH, ZEC.
   - **EVM Chains (EIP-55 Checksum):** Ethereum, Arbitrum, Optimism, Base, Polygon, BNB Chain, Avalanche C-Chain, Linea, Scroll, zkSync Era, Blast, Mantle, Fantom, Gnosis, Cronos.
   - **Memo / Destination Tag Mandatory Chains:** Detects and validates required destination tags and memos for **XRP (Ripple)**, **XLM (Stellar)**, **TON**, **ATOM (Cosmos Hub)**, **EOS**, **HBAR (Hedera)**, **XEM (NEM)**, **KAVA**, **OSMO**, and **INJ** to prevent accidental fund loss on custodial deposits and cross-chain settlements.
   - **Non-EVM L1s:** Solana (`Ed25519` Base58), TRON (`T...` Base58Check), Sui, Aptos, NEAR, Polkadot/Substrate (`SS58`), Cardano (`addr1...`), Monero (`4...`/`8...`).
2. **Anti-Address-Poisoning Similarity Guard**
   - Detects prefix/suffix vanity lookalike addresses commonly used in on-chain zero-value transfer poisoning attacks.
3. **Offline & Zero Telemetry**
   - Runs 100% locally via standalone Windows executable (`multichain-address-validator.exe`) or Node.js CLI with zero network calls.

---

## Usage (Windows Standalone CLI / GUI)

```powershell
# Validate a single blockchain address
multichain-address-validator.exe --chain ETH --address 0x71C7656EC7ab88b098defB751B7401B5f6d8976F

# Validate a memo-required chain (XRP / XLM / TON / ATOM / HBAR)
multichain-address-validator.exe --chain XRP --address rEb8TK3gBgk5auZkwc6sHnwrGVJH8DuaLh --memo 10492819

# Check two addresses for address-poisoning lookalike risk
multichain-address-validator.exe --check-poison 0x71C7656EC7ab88b098defB751B7401B5f6d8976F 0x71C7000000000000000000000000000000d8976F

# List all 46 supported blockchain rules
multichain-address-validator.exe --list-chains
```

## License

Released under the [MIT License](./LICENSE) by the [AllSwap](https://allswap.io) open-source engineering team.
