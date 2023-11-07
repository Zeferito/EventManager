module.exports = {
    trailingComma: 'none',
    tabWidth: 4,
    useTabs: false,
    singleQuote: true,
    endOfLine: 'auto',
    plugins: [require('@trivago/prettier-plugin-sort-imports')],
    importOrderParserPlugins: ['typescript', 'decorators-legacy'],
    importOrder: [
        '^@nestjs/(.*)$',
        '^@core/(.*)$',
        '<THIRD_PARTY_MODULES>',
        '^[./]'
    ],
    importOrderSeparation: true,
    importOrderSortSpecifiers: true
};
