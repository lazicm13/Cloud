<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="CloudService" generation="1" functional="0" release="0" Id="aba5f9e8-20f1-443a-9209-a1562dbb2eb4" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="CloudServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="RedditService_WebRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/CloudService/CloudServiceGroup/LB:RedditService_WebRole:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="NotificationService:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CloudService/CloudServiceGroup/MapNotificationService:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="NotificationService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CloudService/CloudServiceGroup/MapNotificationService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="NotificationServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CloudService/CloudServiceGroup/MapNotificationServiceInstances" />
          </maps>
        </aCS>
        <aCS name="RedditService_WebRole:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CloudService/CloudServiceGroup/MapRedditService_WebRole:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="RedditService_WebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CloudService/CloudServiceGroup/MapRedditService_WebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="RedditService_WebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CloudService/CloudServiceGroup/MapRedditService_WebRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:RedditService_WebRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/CloudService/CloudServiceGroup/RedditService_WebRole/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapNotificationService:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudService/CloudServiceGroup/NotificationService/DataConnectionString" />
          </setting>
        </map>
        <map name="MapNotificationService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudService/CloudServiceGroup/NotificationService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapNotificationServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CloudService/CloudServiceGroup/NotificationServiceInstances" />
          </setting>
        </map>
        <map name="MapRedditService_WebRole:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudService/CloudServiceGroup/RedditService_WebRole/DataConnectionString" />
          </setting>
        </map>
        <map name="MapRedditService_WebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudService/CloudServiceGroup/RedditService_WebRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapRedditService_WebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CloudService/CloudServiceGroup/RedditService_WebRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="NotificationService" generation="1" functional="0" release="0" software="C:\Users\User\OneDrive\Desktop\cloud najnoviji\Cloud\Reddit\CloudService\csx\Debug\roles\NotificationService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;NotificationService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;NotificationService&quot; /&gt;&lt;r name=&quot;RedditService_WebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CloudService/CloudServiceGroup/NotificationServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/CloudService/CloudServiceGroup/NotificationServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/CloudService/CloudServiceGroup/NotificationServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="RedditService_WebRole" generation="1" functional="0" release="0" software="C:\Users\User\OneDrive\Desktop\cloud najnoviji\Cloud\Reddit\CloudService\csx\Debug\roles\RedditService_WebRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;RedditService_WebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;NotificationService&quot; /&gt;&lt;r name=&quot;RedditService_WebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CloudService/CloudServiceGroup/RedditService_WebRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/CloudService/CloudServiceGroup/RedditService_WebRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/CloudService/CloudServiceGroup/RedditService_WebRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="RedditService_WebRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="NotificationServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="NotificationServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="RedditService_WebRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="NotificationServiceInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="RedditService_WebRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="bce8f687-2798-44dd-947c-f3fa2e62f308" ref="Microsoft.RedDog.Contract\ServiceContract\CloudServiceContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="b5860cac-b765-4236-852f-b1e014c24b3e" ref="Microsoft.RedDog.Contract\Interface\RedditService_WebRole:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CloudService/CloudServiceGroup/RedditService_WebRole:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>