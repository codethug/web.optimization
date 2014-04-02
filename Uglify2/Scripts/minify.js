
/* ----------------------------- Custom implementation of minify --------------------------- */

function minify(arguments) {

    var args = JSON.parse(arguments);
    var fileUrls = args.fileUrls;
    var fileContents = args.fileContents;
    var options = args.options;

    options = UglifyJS.defaults(options, {
        spidermonkey: false,
        outSourceMap: null,
        sourceRoot: null,
        inSourceMap: null,
        warnings: false,
        mangle: {},
        output: null,
        compress: {}
    });
    UglifyJS.base54.reset();

    // 1. parse
    var toplevel = null,
        sourcesContent = {};

    if (options.spidermonkey) {
        toplevel = UglifyJS.AST_Node.from_mozilla_ast(files);
    } else {
        if (typeof fileUrls == "string")
            fileUrls = [fileUrls];
        if (typeof fileContents == "string")
            fileContents = [fileContents];

        for (var i = 0; i < fileUrls.length; i++) {
            var url = fileUrls[i];
            var code = fileContents[i];
            sourcesContent[url] = code;
            toplevel = UglifyJS.parse(code, {
                filename: url,
                toplevel: toplevel
            });
        }
    }

    // 2. compress
    if (options.compress) {
        var compress = { warnings: options.warnings };
        UglifyJS.merge(compress, options.compress);
        toplevel.figure_out_scope();
        var sq = UglifyJS.Compressor(compress);
        toplevel = toplevel.transform(sq);
    }

    // 3. mangle
    if (options.mangle) {
        toplevel.figure_out_scope();
        toplevel.compute_char_frequency();
        toplevel.mangle_names(options.mangle);
    }

    // 4. output
    var inMap = options.inSourceMap;
    var output = {};
    if (typeof options.inSourceMap == "string") {
        inMap = options.inSourceMap;
    }

    var result = {};

    try {
        if (options.outSourceMap) {
            output.source_map = UglifyJS.SourceMap({
                file: options.outSourceMap,
                orig: inMap,
                root: options.sourceRoot
            });

            if (options.sourceMapIncludeSources) {
                for (var file in sourcesContent) {
                    if (sourcesContent.hasOwnProperty(file)) {
                        options.source_map.get().setSourceContent(file, sourcesContent[file]);
                    }
                }
            }

        }

        if (options.output) {
            UglifyJS.merge(output, options.output);
        }
        var stream = UglifyJS.OutputStream(output);
        toplevel.print(stream);

        result.code = stream + "";
        result.map = output.source_map + '';
    } catch (e) {
        return e.stack;
    }

    return JSON.stringify(result);
};
