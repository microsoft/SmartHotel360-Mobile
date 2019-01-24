echo "Post Build Script Started"

# Environment variables :
# - TOKEN. You need an AppCenter API token. Instructions on how to get it in https://docs.microsoft.com/en-us/appcenter/api-docs/
# - XAMARIN_UITEST_VERSION. Version of the Xamarin.UITest NuGet package the project is using. Defaults to 2.2.7
# - DEVICES. ID or IDs of devices or device sets previously created in AppCenter. Defaults to "Pixel 2" (7c5a701f)
# - $APP. Name of the app

if [ -z "$TOKEN" ]; then
	echo "ERROR! AppCenter API token is not set. Exiting..."
	exit 1
fi

# DEFAULTS

DEFAULT_XAMARIN_UITEST_VERSION="2.2.7"
DEFAULT_DEVICES="7c5a701f"

if [ -z "$DEVICES" ]; then
	echo "WARNING! 'DEVICES' variable not set. You need to previously create a device set, and specify it here, eg: <project_name>/samsunggalaxys"
	echo "Defaulting to Google Pixel 2 (7c5a701f)"
	DEVICES=$DEFAULT_DEVICES
fi

# INSTALL XAMARIN UITEST TOOLS

if [ -z "$XAMARIN_UITEST_VERSION" ]; then
	XAMARIN_UITEST_VERSION=$DEFAULT_XAMARIN_UITEST_VERSION
	echo "WARNING! XAMARIN_UITEST_VERSION environment variable not set. Setting it to its default. Check the version of Xamarin.UITest you are using in your project"
fi

echo "Xamarin UITest version set to $XAMARIN_UITEST_VERSION"

echo "### Install Xamarin.UITest tools..."
nuget install Xamarin.UITest -ExcludeVersion -Version $XAMARIN_UITEST_VERSION

UI_TEST_TOOLS_DIR="Xamarin.UITest/tools"

# UI TEST PROJECT BUILD

UITestProject=$(find "$APPCENTER_SOURCE_DIRECTORY" -name SmartHotel.Clients.UITests.csproj)
echo UITestProject: $UITestProject

echo "#### Starting UI Test project build..."
msbuild $UITestProject /t:build /p:Configuration=Release

echo "#### Starting test run..."
appcenter test run uitest --app $APP --devices $DEVICES --app-path $APPCENTER_OUTPUT_DIRECTORY/com.microsoft.smarthotel.apk  --test-series "master" --locale "en_US" --build-dir $APPCENTER_SOURCE_DIRECTORY/Source/SmartHotel.Clients.UITests/bin/Release --uitest-tools-dir $UI_TEST_TOOLS_DIR --token $TOKEN --async