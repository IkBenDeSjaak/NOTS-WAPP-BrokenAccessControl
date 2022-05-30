# Nots WAPP Workshop

## Nots Wapp broken access control

You can see the demo of broken access control here.

## Deployment on Azure

Follow the following steps to deploy your ASP.NET Core application with database to Microsoft Azure app platform. You can use your Airbnb app or [this demo](https://github.com/Azure-Samples/msdocs-app-service-sqldb-dotnetcore/archive/refs/heads/main.zip) from Microsoft.

### 1 Create App Service

First you need to create an app service, where your app will be hosted.

1. Go to https://portal.azure.com/
	 - If you have an account with your HAN email, switch to the HAN directory (search for ``Directories + subscriptions`` to switch).
2. Search for ``App Services`` and click ``+ Create``
3. You should now be on the ``Create Web App`` page. Fill it out as following:
	- **Resource group:** create new and give it a name (ex ``Workshop deployment``) so you can easily remove it later
	- **Name:** choose a unique name for your app
	- **Publish:** code
	- **Runtime stack:** ``.Net Core 6 (LTS)``
	- **Operating system:** Windows
	- **Region:** West Europe (or any other region)
	- **App service plan:** Create a new windows plan and choose the ``Free F1`` Sku 
4. Press ``Review + create``, check that you have the correct settings and click create

### 2 Create a database service

The database needs to be created seperatly from the app service.

1. Search for **SQL Servers** and click on ```+ Create`` once you're on the SQL Database page
2. Fill out the page as following:
	- **Project details:** Choose the same subscription and resource group as the app service
	- **Server details:** Enter a unique name for the database (ex ``coreDb``) and select West Europe as region
	- **Authentication:** Select ``Use SQL authentication`` and create a username and password
	- **Compute+storage**: Select Basic service tier and change max data size to 0,5 GB
3. Go to the networking tab and make sure ``Allow Azure services and resources to access this server`` is set to ``Yes``
4. Wait until the database is deployed and click ``Go to resource``

### 3 Create a database

You can now create a new database or use a bacpac with data already filled out.

#### New database

Create a new database by clicking on ``+ Create database`` and fill out the page as following:
	- Choose a name and leave the rest as default
	
#### Import bacpac

1. Search for ``Storage accounts`` and create a new storage account using "Standard performance" and "Geo redundant storage"
2. Create a new container for the bacpac file
3. Press upload on the storage account overview screen and upload the bacpac
4. Go to your database service and press ``Import database``
5. Press ``Select backup``  and choose the storage account and container with the bacpack. Then select the bacpac
6. Make sure to choose the Basic service tier with max data size of 0,5 GB

The importing of the bacpac can easily take 10 minutes, so we'll do something else in the meantime

### 3a Deploy to App service with Visual Studio

You can deploy to Azure directly from Visual Studio.

1. Open your project and select ``Build`` -> ``Publish projectname``
2. Select ``Azure`` as your deployment target
3. Select ``Azure App Service (Windows)`` as your specific target
4. Sign in with your Azure account
5. Select the subscription and resource group where you deployed your App Service. Click Finish and on the next screen click Publish.

 > Make sure you point to the correct url for API calls!

### 3b Deploy to App Service without Visual Studio

First of all, install Azure CLI by following [these](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli#install) instructions. You also need git installed on your system.

1. Run this command with your app name and the resource group that the app service is in.

``az webapp deployment source config-local-git --name <your-app-name> --resource-group <resource-name>``

2. Save these deployment credentials because you'll need them later

``az webapp deployment list-publishing-credentials  --name <your-app-name> --resource-group <resource-name> --query "{Username:publishingUserName, Password:publishingPassword}"``

3. Now add a new remote to the repository you want to publish

``git remote add azure https://<your-app-name>.scm.azurewebsites.net/<your-app-name>.git``

4. Push your code to Azure

``git push azure main:master``

### 4 Connect app to database

1. Enter the name of your database (eg ``coreDb``) in the search field
2. On the database overview page, click on ``Connection strings`` in the sidebar on the left. Save this connection string in eg notepad for now and edit it with your password
3. Now search for the name of the app service (ex ``workshop-deployment``) and select ``Configuration`` in the sidebar
4. Scroll down and select ``+ Create new connection string``
5. Think of a name, enter the connection string from step 2 and select ``SQLserver`` as the type. Click on ``Ok`` and then on ``Save`` at the top of the page

Now we need to setup the app and database to be able to be accessed from your computer.

1. Search for the SQL server you created earlier and select it
2. In the sidebar, select the ``Networking`` and then scroll down and click ``+ Add your client ipv4 address``
3. Click ``Save``
4. Go to the ``appsettings.json`` in the project and add the connection string like this:

```
"ConnectionStrings": {
    "MyDbConnection": "Server=tcp:<your-server-name>.database.windows.net,1433;
        Initial Catalog=coredb;
        Persist Security Info=False;
        User ID=<username>;Password=<password>;
        Encrypt=True;
        TrustServerCertificate=False;"
  }
  ```
  
Now we need to generate the database schema for the app if you haven't used a bacpac.

5. Install dotnet-ef ``dotnet tool install -g dotnet-ef``
6. Create a migration ``dotnet ef migrations add InitialCreate ``
7. Then update the database ``dotnet ef database update --connection <connectionstring>`` and publish the app like before (using visual studio or pushing to git branch)

You should now be able to visit your website!

### Errors

Some solutions to errors that we came across:

 - If you get an ``there was an error trying to log you in: 'cannot read properties of undefined (reading 'tolowercase')'`` then you need to update your packages, put the following code in the .csproject and delete the bin and obj folder
```
<ItemGroup>
	<TrimmerRootAssembly Include="Microsoft.Authentication.WebAssembly.Msal" />
</ItemGroup>
```

## Sources

[Deploy an ASP.NET Core and Azure SQL Database app to Azure App Service - Azure App Service | Microsoft Docs](https://docs.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?tabs=azure-portal%2Cvisualstudio-deploy%2Cdeploy-instructions-azure-portal%2Cazure-portal-logs%2Cazure-portal-resources)
