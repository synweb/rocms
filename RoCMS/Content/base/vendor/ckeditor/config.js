/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
    CKEDITOR.dtd.$blockLimit.a = 1;
    CKEDITOR.dtd.a.div = 1;


    //CKEDITOR.dtd.$inline.div = 1;

    CKEDITOR.dtd.$removeEmpty =
    {
        div: 0,
        a: 0,

        abbr: 0,
        acronym: 0,
        b: 0,
        bdi: 0,
        bdo: 0,
        big: 0,
        cite: 0,
        code: 0,
        del: 0,
        dfn: 0,
        em: 0,
        font: 0,
        i: 0,
        ins: 0,
        label: 0,
        kbd: 0,
        mark: 0,
        meter: 0,
        output: 0,
        q: 0,
        ruby: 0,
        s: 0,
        samp: 0,
        small: 0,
        span: 0,
        strike: 0,
        strong: 0,
        sub: 0,
        sup: 0,
        time: 0,
        tt: 0,
        u: 0,
        "var": 0
    };

    //config.enterMode = CKEDITOR.ENTER_BR;
    config.forcePasteAsPlainText = false; // default so content won't be manipulated on load
    config.basicEntities = true;
    config.entities = true;
    config.entities_latin = false;
    config.entities_greek = false;
    config.entities_processNumerical = false;
    config.allowedContent = true;
    config.autoParagraph = false;
    config.fillEmptyBlocks = false;
    config.height = 400;
};

CKEDITOR.on('instanceReady', function (ev) {

    var writer = ev.editor.dataProcessor.writer;

    // The character sequence to use for every indentation step.
    writer.indentationChars = '\t';

    // The way to close self-closing tags, like <br />.
    writer.selfClosingEnd = ' />';

    // The character sequence to be used for line breaks.
    writer.lineBreakChars = '\n';

    // The writing rules for the <p> tag.
    writer.setRules('p', {
        // Indicates that this tag causes indentation on line breaks inside of it.
        indent: true,

        // Inserts a line break before the <p> opening tag.
        breakBeforeOpen: true,

        // Inserts a line break after the <p> opening tag.
        breakAfterOpen: true,

        // Inserts a line break before the </p> closing tag.
        breakBeforeClose: true,

        // Inserts a line break after the </p> closing tag.
        breakAfterClose: true
    });

    // The writing rules for the <p> tag.
    writer.setRules('div', {
        // Indicates that this tag causes indentation on line breaks inside of it.
        indent: true,

        // Inserts a line break before the <p> opening tag.
        breakBeforeOpen: true,

        // Inserts a line break after the <p> opening tag.
        breakAfterOpen: true,

        // Inserts a line break before the </p> closing tag.
        breakBeforeClose: true,

        // Inserts a line break after the </p> closing tag.
        breakAfterClose: true
    });

    // The writing rules for the <p> tag.
    writer.setRules('a', {
        // Indicates that this tag causes indentation on line breaks inside of it.
        indent: true,

        // Inserts a line break before the <p> opening tag.
        breakBeforeOpen: true,

        // Inserts a line break after the <p> opening tag.
        breakAfterOpen: true,

        // Inserts a line break before the </p> closing tag.
        breakBeforeClose: true,

        // Inserts a line break after the </p> closing tag.
        breakAfterClose: true
    });
});