#!/bin/sh

# Do NOT change the replacement order!
# project GUID: bc9d47e4-823c-45aa-af25-f052eafee17f
# assembly GUID: 8ddcb5e9-86ba-49f1-9f4b-fe4a8f2a4567
for regexp in 's/VARIABLE_NAMESPACE/VARIABLE_NAMESPACE/g' 's/VARIABLE_NAME/VARIABLE_NAME/g' 's/VARIABLE_ASSEMBLY_NAME/VARIABLE_ASSEMBLY_NAME/g' 's/VARIABLE_AUTHOR/VARIABLE_AUTHOR/g' 's/VARIABLE_YEAR/VARIABLE_YEAR/g' 's/VARIABLE_SUPPORTED_VERSION/VARIABLE_SUPPORTED_VERSION/g' 's/VARIABLE_DESCRIPTION/VARIABLE_DESCRIPTION/g' 's/VARIABLE_URL/VARIABLE_URL/g' 's/bc9d47e4-823c-45aa-af25-f052eafee17f/bc9d47e4-823c-45aa-af25-f052eafee17f/g' 's/8ddcb5e9-86ba-49f1-9f4b-fe4a8f2a4567/8ddcb5e9-86ba-49f1-9f4b-fe4a8f2a4567/g'; do
	echo Applying \'$regexp\'…
	find . \( -type f -wholename "$0" \) -o \( -type d -name '.git' -prune \) -o -type f -print0 | xargs -0 sed -i "$regexp"
done
