---
title: Register a Dataverse service endpoint
description: Learn how to define a connection to an external system by registering a Dataverse service endpoint. In this scenario, we establish a hybrid relay connection to an Azure listener app using the ServiceBus.
author: phecke
ms.author: pehecke
ms.subservice: dataverse-developer
ms.topic: how-to
ms.date: 08/19/2024
---

# Register a Dataverse service endpoint

 In this article, you use the Plug-in Registration tool (PRT) to register a Dataverse service endpoint. You configure the endpoint to use the default system plug-in, a REST contract, and an Azure hybrid relay connection.

A service endpoint contains Dataverse configuration information for an external system connection. In this case, we're configuring the service endpoint to access an Azure hybrid relay connection using the ServiceBus. This enables Dataverse to post the execution context of its main operation on the service bus and read by an Azure listener app.

## Prerequisites

Here's a list of prerequisites for registering a service endpoint.

- Plug-in Registration tool installed on your development computer.
- Azure account with a license to create Service Bus entities.
- Configured Service Bus namespace.
- Configured Service Bus hybrid relay connection with Send and Listen SAS policy permissions.

Detailed information on configuring an Azure namespace and hybrid connection can be found in this article: [Get started with Relay Hybrid Connections HTTP requests in .NET](https://learn.microsoft.com/azure/azure-relay/relay-hybrid-connections-http-requests-dotnet-get-started).

To install the PRT, see [Dataverse development tools](https://learn.microsoft.com/power-apps/developer/data-platform/download-tools-nuget).

## Obtain Azure namespace and hybrid connection information

Record the Azure connection information needed to create a service endpoint.

1. Sign in to the Azure [portal](https://portal.azure.com).
1. Choose **ServiceBus** from the Azure services group or in the navigation pane.
1. On the **Home** page, under the **Resources** group, select the relay that you configured.
1. Select **Settings** > **Shared access policies**, and then choose **RootManagedSharedAccessKey**.
1. Record the values of the **Primary Key** and the **Primary Connection String** for the SAS policy named "RootManageSharedAccessKey".
1. On the relay page, choose **Overview**, then near the page bottom below Hybrid Connections selects the hybrid connection you configured.
1. On the **Hybrid Connection** page, record the **Hybrid Connection Url** value.

You can sign out of the portal now.

## Launch the Plug-in Registration tool

Launch the PRT using the Power Platform CLI.

1. In a Visual Studio Code terminal window or in the standalone CLI, execute the following command to launch the PRT.

    `pac tool prt`

## Register a service endpoint and step

Create a service endpoint using the PRT by following these instructions.

1. In the opening window of the PRT, select **+CREATE NEW CONNECTION** and sign in to your target environment.
1. In the tab of your environment, choose **Register** > **Register New Service Endpoint**.
1. Check the option **Let's Start with the connection string...** and paste the primary connection string you saved from the Azure portal. The string starts with "Endpoint=sb://".
1. Select **Next**.
1. Fill out the **Service Endpoint Registration** form. Most of the data is auto populated from the primary connection string you provided.
1. Be sure to set the **Designation Type** as **Rest**, set **Path** to the name of your hybrid connection, and add the path to the end of the **NameSpace Address**.<p/>
    !["Service endpoint registration."](media/azure-service-endpoint-registration.png)
    <!-- :::image type="content" source="media/azure-service-endpoint-registration.PNG" alt-text="Service endpoint registration."::: -->
1. Select **Save**.

Next, we're going to add a step to the service endpoint. The step indicates under what conditions the remote execution context is posted to the service bus.

1. Select **Register** > **Register New Step**.
1. Fill out the displayed form as show in the figure using appropriate values for your configuration.<p/>
    !["Step registration."](media/azure-service-endpoint-step.png)
    <!-- :::image type="content" source="media/azure-service-endpoint-step.png" alt-text="Step registration."::: -->
1. Select **Register New Step**.

Use the appropriate message, primary entity, service endpoint event handler, etc. for your setup. Leave the stage, execution mode, and deployment values as shown.

If you aren't ready to test sending the Dataverse remote execution context to Azure at this time, you can disable the step and then enable it when ready.

1. Locate your registered service endpoint in the **Registered Plugins** list and expand it by selecting the arrow next to the item.
1. Expand the plug-in item within the service endpoint.
1. Right-click the step item within the plug-in and choose **Disable**.

## Related content

- [Azure integration](https://learn.microsoft.com/power-apps/developer/data-platform/azure-integration)
- [Use a hybrid relay connection](azure-hybrid-relay-connection.md)
