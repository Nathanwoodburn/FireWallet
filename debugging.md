# Debugging Info

## App startup errors


## Node (HSD) errors
If you have selected to run the internal node you can check these steps to see if you can fix the issue.

1. Check the HSD directory in `%appdata%\FireWallet\` (`C:\Users\{username}\AppData\Roaming\FireWallet\HSD`)
2. Try running `npm install` in the HSD directory (could have failed in the install process)
3. Look in the HSD logs for errors in `C:\Users\{username}\.hsd\debug.log`


## Ledger errors
If you are having issues with Ledger you can check these steps to see if you can fix the issue.

1. Check the Ledger directory in `%appdata%\FireWallet\` (`C:\Users\{username}\AppData\Roaming\FireWallet\hsd-ledger`)
2. Try running `npm install` in the Ledger directory (could have failed in the install process)
3. Try running `node bin\hsd-ledger createaddress --api-key {api-key} -w {cold wallet name}` in the Ledger directory. This will get you to verify your address on the ledger device. If this fails you have some bigger issue.