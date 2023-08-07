<br>

### Due to a recent issue with bids being sent with the wrong address included with the lockup please update HSD.
This can be done if you are using the internal node.
1. Close the wallet if it is open
2. Open a terminal and run
```bat
cd %appdata%\FireWallet\hsd
git pull
```
3. Verify the fix is pulled by running
```bat
if exist "test\wallet-importnonce-test.js" (echo Fix pulled) else (echo Fix isn't pulled)
```


<br><br>

# FireWallet
Experimental wallet for Handshake chain  
Info about the project can be found at https://firewallet or https://firewallet.au

## Installation
### Dependencies
You will need .net desktop installed. You can download it from [here](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.18-windows-x64-installer).  

You will also need Node, NPM, and git installed if you want to use the internal HSD or Ledger wallets.  
[Git](https://git-scm.com/downloads)  
[Node](https://nodejs.org/en/download/)  
[NPM](https://docs.npmjs.com/downloading-and-installing-node-js-and-npm)

### From Releases
You can install the latest release from [here](https://github.com/Nathanwoodburn/FireWallet/releases/).

Here are some videos showing how to install and use FireWallet:
Install with a running Bob wallet: https://cloud.woodburn.au/s/pS8e2oQDidrMJWx
Install with internal HSD: https://cloud.woodburn.au/s/trwd8TyxbDfqaxF (You NEED to have Node, NPM, and git installed to use internal HSD)

### Build from source
You can build from source by cloning this repo.  
You will need Visual Studio (recommend a recent version) and .net desktop development tools installed.

## Setup
### Create a new wallet
You can create or import a wallet by clicking on the `New` button from the login screen.  
You will need to enter a password to encrypt your wallet.  
This password will be used to encrypt your wallet and to login to your wallet.  
**At the moment exporting your Seed Phase does not work**

![Alt text](assets/account_new.png)


## First time setup
When you first open the wallet you will be prompted to set your node settings.

You can either connect to an existing HSD (or Bob) node or you can run your own node.  
If you want to run your own node you should select the `Run HSD` option.  
This will take a few minutes to download and install HSD.

You can get the API key from the HSD launch command or 
in Bob wallet under settings > Wallet > API key.  
If you change this key in HSD or Bob you will need to update it in FireWallet.  

![Node settings](assets/node_settings.png)  
If you need to change your Node settings you can edit the file in `%appdata%\FireWallet\node.txt`

<br><br><br><br>
# Usage

Login to your wallet by entering your password (the same as the Bob password).  
This password will be stored in memory until you Logout or close the wallet.  
This means you will not need to enter your password again until you close the wallet.  

# Portfolio
The portfolio page shows your current balance.  
The locked balance is the amount of HNS that is currently locked in auctions.  
This also includes the amount of HNS that is locked in closed auctions (aka the spent bid).  

This page shows a list of your transactions.  
Clicking on a Hash will open the transaction in an explorer.  
You can change the number of transactions shown in the `portfolio-tx:` settings.

<br>

![Portfolio](assets/portfolio.png)

<br><br>
## Sending HNS
![Send](assets/send_hns.png)
This page lets you send HNS to Handshake addresses or domains using [HIP-02](https://github.com/handshake-org/HIPs/blob/master/HIP-0002.md).  
To use HIP-02 you need to have HSD resolver (or any HNS compatible DNS resolver) listening on port 5350 (default HSD port).  
To enter a domain to use HIP-02 you need to prefix the domain with `@` (eg. `@nathan.woodburn`).

## Receiving HNS or Domains
The receive page shows your current HNS address.  
This is the address you can give to people to send you HNS or domains.  
This address will change every time you receive HNS or domains to prevent address reuse.

![Receive](assets/receive.png)


## Domains
The domains page shows a list of your domains.  
It also lets you search for domains.
Clicking on a domain will open the domain in the domain windows.

![Domains](assets/domains.png)

The domain windows shows the details of the domain.  
This window also lets you auction and manage domains.  

![Domain_Search](assets/domain_search.png)
![Domain_Manage](assets/domain_manage.png)
## Manage Domains
You can update the records of your domains by clicking on the `Edit DNS` or `Edit in Batch` buttons.  
This will open a window where you can edit the records of your domain.  
After you have made your changes you can click the `Send` button to either send the transaction to the network or to the batch window.


![DNS Editor](assets/dns.png)



# Batching
The batch window lets you send multiple transactions at once.  
You can add transactions to the batch from the domain window or the DNS editor.  

![Batch](assets/batch.png)


## Importing
You can also import a list of domains to the batch window.  
The "CANCEL" transaction type is used to cancel an transfer.

Please not that the import syntax for BIDs is BID,LOCKUP where LOCKUP is (BID+BLIND)
![Batch Import](assets/batch_import.png)

## Exporting
You can export the batch to a file.  
This file will store the transaction type, name, and any data needed to send the transaction.  
You can then import this file to send the transactions.  
An example of this file can be found [here](example-configs/batch.txt).




# Ledger
You can use a Ledger device to sign transactions.  
You need to have Node, NPM, and git installed to use Ledger.  
The Ledger components are not included in the app.  
These will install when you first send HNS (not domains) from a Ledger.


# Settings
FireWallet uses a few different settings files.  
They are stored in `%appdata%\FireWallet\` (`C:\Users\{username}\AppData\Roaming\FireWallet\`)

## settings.txt
This file stores the user settings for the application.  
If you want to change the default HIP-02 resolver you can add these settings
```yaml
hip-02-ip: 127.0.0.1
hip-02-port: 5350
```


## node.txt
This file stores the node (HSD/Bob connection) settings.  
The Network is the network you want to connect to (default is `0` for Mainnet).  
If you delete this file, FireWallet will show the node setup screen on next startup.

You can set a custom HSD launch command by setting the `hsd-command` key.  
The default launch is the same as this
```yaml
HSD-command: {default-dir} --agent=FireWallet --index-tx --index-address --api-key {key} --prefix {Bob}
```
The `{default-dir}` will be replaced with the HSD directory `%appdata%\FireWallet\hsd\`.  
The `{key}` will be replaced with the API key from the node.txt file.  
The `{Bob}` will be replaced with the Bob wallet HSD data directory `%appdata%\Bob\hsd_data\` this is used to sync FireWallet with Bob's accounts and also stops you needing to sync the chain twice.

Other settings are here. These are the default and if they are not in the file they revert to these values.
```yaml
HideScreen: True # Hide the HSD terminal screen (Set to False for higher reliability)
Timeout: 10 # The time in seconds to wait for any API request
```

## theme.txt
This file stores the theme settings.  
The theme is the color scheme of the application.  
The `transparent-mode` key is used to enable or disable transparent modes.  
There are 4 modes: `off` is disabled, `mica` is windows app style, `key` is to make 1 colour transparent, and `percent` is to set the opacity of the window.

## log.txt
This file stores the logs for the application.  
You should check this file if you have any issues with the application.

# Thanks
Thanks to @ponderingken (https://github.com/ponderingken) for designing the logo and splash screen.



# Support
If you have any issues with the application you can open an issue on GitHub or contact me on Discord (NathanWoodburn on most Handshake servers).

If you would like to support this project you can find out how at https://nathan.woodburn.au/#donate or you can help by contributing to the project on GitHub.  
Also you can send HNS directly to `@firewallet`