#Host ASP.NET Core on Linux with Apache server.
This guide explains setting up a production-ready ASP.NET Core environment on an Ubuntu 16.04 server. These instructions likely work with newer versions of Ubuntu, but the instructions haven't been tested with newer.
##Prerequisites
1. Running Linux ubuntu server with sudo privileged.
2. Installed ASP.NET Core.
3. Installed Apache2 server.

##ASP.NET core installation.
1. Open terminal and execute below command
```
wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
```

2. Install ASP.NET runtime.
```
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install aspnetcore-runtime-2.1
```
3. Test your .Net Installation.you should get dotnet command help 
`dotnet` 
If you get anything wrong . Please visit https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache?view=aspnetcore-2.1&tabs=aspnetcore2x

##Install Apache2 server on system
The Apache HTTP Web Sever (Apache) is an open source web application for deploying web servers
1.Before insallation check if required packages are uptodate. Run following command
```
sudo apt-get update && sudo apt-get upgrade
```
2.Excute below command to install apache2
```
sudo apt-get install apache2 apache2-doc apache2-utils
```
3.Test if you have successfully installed Apache2.Run following command.
```service apache2 status```
##publish ASP.NET project.
Run the following command to publish your ASP.NET application.
```dotnet publish --configuration Release```
##Configure your proxy server.
.Net Application is already running on *kestrel* webserver with different port need to be served via proxy with help of *apache server*.follow these steps one by one .

![data/admin/2018/9/apache-proxy-kestrel.png](/data/admin/2018/9/apache-proxy-kestrel.png)
1. Enabling Necessary Apache Modules. 
```sudo a2enmod proxy
sudo a2enmod proxy_http
sudo a2enmod headers
sudo systemctl restart apache2
```
2. Create a new *VirtualHost* for website.
* Create new .conf by copying existing .conf file.
```sudo cp /etc/apache2/sites-available/000-default.conf /etc/apache2/sites-available/blog.apexpath.com.conf```
* Open blog.apexpath.com.conf file in editor.
```sudo vim /etc/apache2/sites-available/blog.apexpath.com.conf```
* Copy below content or change as per your requirment.
```	<VirtualHost *:*>
	    RequestHeader set "X-Forwarded-Proto" expr=%{REQUEST_SCHEME}
	</VirtualHost>
	<VirtualHost *:80>
        ProxyPreserveHost On
        ProxyPass / http://127.0.0.1:5000/
        ProxyPassReverse / http://127.0.0.1:5000/
        ServerName blog.apexpath.com
        ServerAlias *.apexpath.com
        ErrorLog ${APACHE_LOG_DIR}/error.log
        CustomLog ${APACHE_LOG_DIR}/access.log combined
    </VirtualHost>```
* save configuration and enable this website configuration by executing below command.
```sudo a2ensite blog.apexpath.com```
* reload apache configuration
```sudosystemctl restart apache2```

Now you should be able to visit your ASP.NET website with apache. For more information visit
https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache?view=aspnetcore-2.1&tabs=aspnetcore2x

