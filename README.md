# TeslaGateControl
A Exiled V2 mod that allows users to control what Roles or Cards allow the ability to pass through tesla gates without setting them off
## Installation
Place the TeslaGateControl.dll into your Exiled/Plugins Folder and the mod will start next time you launch the server
## Config
The Install is rather lite on configeration options but the ones that will be populated are described below

Config Item | Description | Example Input
----------- | ----------- | ------------- 
IsEnabled | Enables or Disables the mod | true
cardMode | Switchs between Role and Card determining what allows access through tesla gates | true
allowedRoles | Lists the allowed role names or ids to be used to determine who gets access through tesla gates | [ClassD, 15, 4]
allowedCards | Lists the allowed card names or ids that are used to determine who gets access through the tesla gates. Note: Ids or items that are not keycards will not be allowed by the system | [0, KeycardGuard, 6]
