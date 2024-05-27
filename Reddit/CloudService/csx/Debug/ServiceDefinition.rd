﻿<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="CloudService" generation="1" functional="0" release="0" Id="d674161f-0937-40ec-bddc-2290b5e72a24" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
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
          <role name="RedditService_WebRole" generation="1" functional="0" release="0" software="C:\Users\HP EliteBook 840-G2\Desktop\Projekat3\Cloud-main-24.05\Cloud-main\Reddit\CloudService\csx\Debug\roles\RedditService_WebRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;RedditService_WebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;RedditService_WebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
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
        <sCSPolicyFaultDomain name="RedditService_WebRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="RedditService_WebRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="8a1ecc69-31b5-4c25-8cb0-6c7cd6d900ec" ref="Microsoft.RedDog.Contract\ServiceContract\CloudServiceContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="26c93d96-c366-4c8e-96df-e800d2b9c3ef" ref="Microsoft.RedDog.Contract\Interface\RedditService_WebRole:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CloudService/CloudServiceGroup/RedditService_WebRole:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>