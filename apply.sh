#!/bin/sh

# Do NOT change the replacement order!
# project GUID: bc9d47e4-823c-45aa-af25-f052eafee17f
# assembly GUID: 8ddcb5e9-86ba-49f1-9f4b-fe4a8f2a4567
for regexp in 's/VARIABLE_NAMESPACE/Polyipseity.UnlimitedShortCircuitExplosion/g' 's/VARIABLE_NAME/Unlimited Short Circuit Explosion/g' 's/VARIABLE_ASSEMBLY_NAME/UnlimitedShortCircuitExplosion/g' 's/VARIABLE_AUTHOR/polyipseity/g' 's/VARIABLE_YEAR/2022/g' 's/VARIABLE_SUPPORTED_VERSION/1.3/g' 's/VARIABLE_DESCRIPTION//g' 's/VARIABLE_URL//g' 's/bc9d47e4-823c-45aa-af25-f052eafee17f/f46968db-97a0-42d4-b1d4-48ae0b833918/g' 's/8ddcb5e9-86ba-49f1-9f4b-fe4a8f2a4567/ee5eb0d4-f7f4-47dc-b27b-20762e844060/g'; do
	echo Applying \'$regexp\'…
	find . \( -type f -wholename "$0" \) -o \( -type d -name '.git' -prune \) -o -type f -print0 | xargs -0 sed -i "$regexp"
done
