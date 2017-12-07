from recommonmark.parser import CommonMarkParser

source_parsers = {
    '.md': CommonMarkParser,
    '.workbook': CommonMarkParser
}

source_suffix = ['.workbook', '.md']
