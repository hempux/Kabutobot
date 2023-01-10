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
add a user with no homefolder and no password that will be used to run the systemd service.
´´´´ bash
sudo useradd teamsbot
´´´´

#### Systemd configuration file
Next, create a [systemd service file](https://www.freedesktop.org/software/systemd/man/systemd.service.html)  
the ¨User¨ parameter should be the same as the user created in the previous step.

````
[Unit]
Description=Kabutobot
[Service]
User=teamsbot
WorkingDirectory=/app/kabutobot
ExecStart=/app/kabutobot/KabutoBot
Type=simple
TimeoutStopSec=10
[Install]
WantedBy=multi-user.target
````
The above example expects the `executable`(the program) that gets started to be `named KabutoBot` (Uppercase `K` and `B`), and be placed in all lowercase `/app/kabutobot`

Since the minimal installation lacks a text editor (unless you installed nano), we can use `curl` to download the above file, and if needed make changes with `sed`  
```` bash
curl https://raw.githubusercontent.com/hempux/Kabutobot/main/examples/systemd.service -o kabuto.service
````
inspect the contents of the file with ´cat´ to make sure that the download completed successfully.
```` bash
cat kabuto.service
````

If we dont want to make changes to the file using `sed` go ahead and move it to the systemd folder and enable it so that the bot runs as a service and starts automatically.
````
sudo mv kabuto.service /etc/systemd/user/kabuto.service
````
Next, enable it using `systemctl enable`
```` bash
sudo systemctl enable /etc/systemd/user/kabuto.service
````
Once its enabled we can start the service with `systemctl start` and then use `journalctl` to check the system logs to see if our application has started.

```` bash
sudo systemctl start kabuto.service
sudo journalctl -u kabuto.service
