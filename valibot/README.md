# Valibot Schemas (`multichain-memo-address-validator/valibot`)

Zero-dependency **Valibot** schemas for validating multi-chain cryptocurrency wallet addresses (EVM, Bitcoin, Solana, XRP, Stellar, Cosmos, TON, TRON, Sui) and mandatory custodial Memo / Destination Tag fields.

- **Interactive Multi-Chain Address & Memo Validator**: [https://allswap.io/tools/address-validator](https://allswap.io/tools/address-validator)
- **AllSwap Cross-Chain Aggregator**: [https://allswap.io](https://allswap.io)
- **Documentation**: [https://allswap.io/docs](https://allswap.io/docs)

```ts
import * as v from 'valibot'
import { EvmAddressSchema, MultiChainTransferSchema } from 'multichain-memo-address-validator/valibot'

const ethAddress = v.parse(EvmAddressSchema, '0x71C95911E9a5D330f4D621842EC243EE1343292e')
```
