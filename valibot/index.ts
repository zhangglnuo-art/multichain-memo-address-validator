import * as v from 'valibot'

export const EvmAddressSchema = v.pipe(
  v.string(),
  v.trim(),
  v.regex(/^0x[a-fA-F0-9]{40}$/, 'Invalid EVM (Ethereum / Arbitrum / Base / Optimism / Polygon / BNB) address')
)

export const BitcoinAddressSchema = v.pipe(
  v.string(),
  v.trim(),
  v.regex(
    /^(1[a-km-zA-HJ-NP-Z1-9]{25,34}|3[a-km-zA-HJ-NP-Z1-9]{25,34}|bc1[qpzry9x8gf2tvdw0s3jn54khce6mua7l]{25,87})$/i,
    'Invalid Bitcoin (Legacy / SegWit / Taproot Bech32m) address'
  )
)

export const SolanaAddressSchema = v.pipe(
  v.string(),
  v.trim(),
  v.regex(/^[1-9A-HJ-NP-Za-km-z]{32,44}$/, 'Invalid Solana Ed25519 Base58 address')
)

export const XrpAddressSchema = v.pipe(
  v.string(),
  v.trim(),
  v.regex(/^r[1-9A-HJ-NP-Za-km-z]{24,34}$/, 'Invalid XRP Ledger address')
)

export const XrpDestinationTagSchema = v.pipe(
  v.string(),
  v.trim(),
  v.regex(/^[0-9]{1,10}$/, 'XRP custodial transfers require a numeric Destination Tag')
)

export const StellarAddressSchema = v.pipe(
  v.string(),
  v.trim(),
  v.regex(/^G[A-Z2-7]{55}$/, 'Invalid Stellar Lumens StrKey address')
)

export const CosmosAddressSchema = v.pipe(
  v.string(),
  v.trim(),
  v.regex(/^cosmos1[qpzry9x8gf2tvdw0s3jn54khce6mua7l]{38}$/, 'Invalid Cosmos Hub Bech32 address')
)

export const TonAddressSchema = v.pipe(
  v.string(),
  v.trim(),
  v.regex(/^((EQ|UQ)[A-Za-z0-9_-]{46}|(0|-1):[a-fA-F0-9]{64})$/, 'Invalid TON address')
)

export const TronAddressSchema = v.pipe(
  v.string(),
  v.trim(),
  v.regex(/^T[1-9A-HJ-NP-Za-km-z]{33}$/, 'Invalid TRON TVM address')
)

export const SuiAddressSchema = v.pipe(
  v.string(),
  v.trim(),
  v.regex(/^0x[a-fA-F0-9]{64}$/, 'Invalid Sui 32-byte address')
)

export const MultiChainTransferSchema = v.object({
  chain: v.picklist(['EVM', 'BTC', 'SOL', 'XRP', 'XLM', 'ATOM', 'TON', 'TRX', 'SUI']),
  address: v.pipe(v.string(), v.trim(), v.minLength(12)),
  memo: v.optional(v.pipe(v.string(), v.trim())),
  trustedReferenceAddress: v.optional(v.pipe(v.string(), v.trim()))
})
