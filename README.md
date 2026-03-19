### Project scope

I'm using this as a "playground" to improve domain-specific (EU data formats, [NeTEx](https://github.com/NeTEx-CEN/NeTEx), [SIRI](https://github.com/SIRI-CEN/SIRI), [DATEX](https://docs.datex2.eu/v3.7/downloads/modelv37/#datex-ii-xml-schema) in particular) XSD validation.

:warning: Do not expect any stability from this project, as it is very much exploratory at this point.

Notes are mostly for myself at this point, too.

Assumptions:
- `dotnet` is installed (`mise install` will work thanks to `.tool-versions` here)
- You unpack the NeTEx zip archive yourself on the disk
- You have a local copy (`git clone` for `NeTEx` for instance) of the XSD you want to validate against

Notes:
- Current testing shows very significant speed improvements (sometimes 3x faster) over `xmllint` XSD validation on my machine
- Equivalence of the errors of this validator versus `xmllint` has not yet been stricly compared (output is structured differently)
- This will likely move to be an Elixir project, leveraging C#/F# via a specific wrapped module (NIF, "Port"...)

### Clone a version of the NeTEx XSD

```
mkdir -p data/xsd
git clone --branch v2.0.0 --depth 1 https://github.com/NeTEx-CEN/NeTEx.git data/xsd/netex-v2.0.0
```

### Download a bit of data

You can download a smallish NeTEx archive at https://transport.data.gouv.fr/resources/82368:

```
curl -L -o data/netex/82368.zip https://transport.data.gouv.fr/resources/82368/download
unzip data/netex/82368.zip -d data/netex/82368.zip.unpack
```

Then call XSD validation with:

```
dotnet fsi xsd-validate.fsx data/xsd/netex-v2.0.0/xsd/NeTEx_publication.xsd data/netex/82368.zip.unpack

# with logs
dotnet fsi xsd-validate.fsx data/xsd/netex-v2.0.0/xsd/NeTEx_publication.xsd data/netex/82368.zip.unpack | tee data/logs/fsx.82368.log
```

### Equivalent call with `xmllint`

```
xmllint --schema data/xsd/netex-v2.0.0/xsd/NeTEx_publication.xsd --noout data/netex/82368.zip.unpack/**/*.xml

# with logs
xmllint --schema data/xsd/netex-v2.0.0/xsd/NeTEx_publication.xsd --noout data/netex/82368.zip.unpack/**/*.xml 2>&1 | tee data/logs/xmllint.82368.log
```

(this is 3 times slower on my tests - will share more benchmarks at some point)