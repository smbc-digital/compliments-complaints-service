# Secrets should be injected into the app from secure sources and never be commited to git unencrypted.
```json
{
    "TokenAuthentication": {
        "Key": "Your secret token here"
    },
    "ElasticsearchAwsSecretsConfiguration": {
        "AccessKey": "[AWS Access Key]<string>",
        "SecretKey": "[AWS Secret Key]<string>"
    },
    "Availability": {
        "Key": "Your secret token here"
    }
}
```json