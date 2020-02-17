#!/bin/sh

# Decrypt the file
mkdir $HOME/secrets
# --batch to prevent interactive command --yes to assume "yes" for questions
gpg --quiet --batch --yes --decrypt --passphrase="$PASSPHRASE" \
--output $HOME/secrets/cachesheet-accountservice.json test/CacheSheet.Tests.Interop/cachesheet-accountservice.json.gpg
