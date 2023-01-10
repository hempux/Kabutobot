### Configuring linux daemon

#### Apache2 + certbot
Install apache2 and certbot(with the apache2 module)
```` bash
apt-get install apache2 python3-certbot-apache -y
````

##### update apache2 configuration
use the `nano` text editor to update the apache2 configuration so that we only listen to the `fqdn` value   
that was ouput by the arm tamplate (if that was used, otherwise use your own hostname)  
Because the minimal image has no text editor we need to use `sed` to replace the ServerName value in the configuration.  
If you want to use a text editor instead i suggest running `sudo apt-get install nano -y`
##### Update config with sed
```` bash 
sudo sed -i 's/1#ServerName www.example.com/kabutobot2.swedencentral.cloudapp.azure.com/' /etc/apache2/sites-enabled/default.conf
````
##### Update with nano
```` bash
sudo apt-get install nano -y
sudo nano /etc/apache2/sites-enabled/default.conf
````
Remove the `#` character before ServerName and replace `www.exmaple.com` with your `fqdn` (or the adress you want to use for the bot if you are using your own)  
Press `ctrl+x`, type ´y´to save, then `Enter` to exit back to the terminal.

##### Add the service user
