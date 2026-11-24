#!/usr/bin/env node
"use strict";

const CHAIN_RULES = {
  BTC: { name: "Bitcoin", pattern: /^(bc1[ac-hj-np-z02-9]{11,71}|[13][a-km-zA-HJ-NP-Z1-9]{25,34})$/, requiresMemo: false },
  ETH: { name: "Ethereum (EVM)", pattern: /^0x[a-fA-F0-9]{40}$/, requiresMemo: false },
  ARB: { name: "Arbitrum One", pattern: /^0x[a-fA-F0-9]{40}$/, requiresMemo: false },
  OP: { name: "Optimism", pattern: /^0x[a-fA-F0-9]{40}$/, requiresMemo: false },
  BASE: { name: "Base", pattern: /^0x[a-fA-F0-9]{40}$/, requiresMemo: false },
  MATIC: { name: "Polygon", pattern: /^0x[a-fA-F0-9]{40}$/, requiresMemo: false },
  BSC: { name: "BNB Smart Chain", pattern: /^0x[a-fA-F0-9]{40}$/, requiresMemo: false },
  AVAX: { name: "Avalanche C-Chain", pattern: /^0x[a-fA-F0-9]{40}$/, requiresMemo: false },
  MANTLE: { name: "Mantle", pattern: /^0x[a-fA-F0-9]{40}$/, requiresMemo: false },
  SOL: { name: "Solana", pattern: /^[1-9A-HJ-NP-Za-km-z]{32,44}$/, requiresMemo: false },
  TRX: { name: "TRON", pattern: /^T[1-9A-HJ-NP-Za-km-z]{33}$/, requiresMemo: false },
  XRP: { name: "XRP Ledger", pattern: /^r[1-9A-HJ-NP-Za-km-z]{24,34}$/, requiresMemo: true, memoName: "Destination Tag", memoPattern: /^\d{1,10}$/ },
  XLM: { name: "Stellar", pattern: /^G[A-Z2-7]{55}$/, requiresMemo: true, memoName: "Memo", memoPattern: /^[a-zA-Z0-9\-_]{1,28}$/ },
  TON: { name: "TON Network", pattern: /^(EQ|UQ)[A-Za-z0-9_\-]{46}$/, requiresMemo: true, memoName: "Comment/Memo", memoPattern: /^.{1,120}$/ },
  ATOM: { name: "Cosmos Hub", pattern: /^cosmos1[ac-hj-np-z02-9]{38}$/, requiresMemo: true, memoName: "Memo", memoPattern: /^.{1,256}$/ },
  EOS: { name: "EOS Network", pattern: /^[a-z1-5.]{1,12}$/, requiresMemo: true, memoName: "Memo", memoPattern: /^.{1,256}$/ },
  HBAR: { name: "Hedera", pattern: /^0\.0\.\d{1,10}$/, requiresMemo: true, memoName: "Memo", memoPattern: /^.{1,100}$/ },
  SUI: { name: "Sui", pattern: /^0x[a-fA-F0-9]{64}$/, requiresMemo: false },
  APT: { name: "Aptos", pattern: /^0x[a-fA-F0-9]{64}$/, requiresMemo: false },
  NEAR: { name: "NEAR Protocol", pattern: /^([a-f0-9]{64}|[a-z0-9_\-]{2,64}\.near)$/, requiresMemo: false }
};

function validateAddress(chain, address, memo) {
  const rule = CHAIN_RULES[String(chain || "").toUpperCase()];
  if (!rule) return { valid: false, error: "UNSUPPORTED_CHAIN" };
  const addressValid = rule.pattern.test(String(address || ""));
  if (!addressValid) return { valid: false, error: "INVALID_ADDRESS_FORMAT", chain: rule.name };
  if (rule.requiresMemo && memo !== undefined && memo !== "") {
    if (!rule.memoPattern.test(String(memo))) {
      return { valid: false, error: "INVALID_MEMO_FORMAT", memoName: rule.memoName };
    }
  }
  return {
    valid: true,
    chain: rule.name,
    requiresMemo: rule.requiresMemo,
    memoWarning: rule.requiresMemo && !memo ? `${rule.name} deposits typically require a ${rule.memoName}` : null
  };
}

function detectAddressPoisoning(addrA, addrB) {
  const a = String(addrA || "").toLowerCase();
  const b = String(addrB || "").toLowerCase();
  if (!a || !b || a === b) return { poisoned: false };
  if (a.length >= 12 && b.length >= 12 && a.slice(0, 6) === b.slice(0, 6) && a.slice(-4) === b.slice(-4)) {
    return { poisoned: true, prefix: a.slice(0, 6), suffix: a.slice(-4) };
  }
  return { poisoned: false };
}

module.exports = { CHAIN_RULES, validateAddress, detectAddressPoisoning };
