#OCR Bot

## Purpose
To help people translate images or just get the text from a given image

## Usage
You can either invite the running bot using [this link](https://discord.com/oauth2/authorize?client_id=758358158799405136&scope=bot) or self host with the instructions below

### Setup
```sh
git clone git@github.com:Jirubizu/OCRBot.git
cd OCRBot/
// Create a config.json with the entires defined below
dotnet run
```
### Config.json
```json
{
    "token": "",
    "mongo_db_uri": "",
    "mongo_db_name": "",
    "ocr_token" : "",
    "translate_base_url" : ""
}
```