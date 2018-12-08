// ReSharper disable Es6Feature
const webpack = require("webpack");

const path = require("path");
const buildPath = path.join(__dirname, "./wwwroot");
const webpackRoot = path.join(__dirname);

const ExtractTextPlugin = require("extract-text-webpack-plugin");
const CleanWebpackPlugin = require("clean-webpack-plugin");
const CopyWebpackPlugin = require("copy-webpack-plugin");

module.exports = {
    entry: {
        backoffice: "./Content/index.js"
    },
    output: {
        path: buildPath,
        publicPath: "/",
        filename: "[name].min.js"
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                loader: "babel-loader"
            },
            {
                test: /\.(otf|png|jpe?g|svg|woff|woff2|eot|ttf|gif)(\?\S*)?$/,
                loader: "file-loader",
                options: {
                    name: "[name].[ext]"
                }
            },
            {
                test: /\.(scss)$/,
                loader: ExtractTextPlugin.extract({
                    use: [
                        {
                            loader: "css-loader",
                            options: {
                                sourceMap: true,
                                importLoaders: 2
                            }
                        },
                        {
                            loader: "postcss-loader",
                            options: {
                                sourceMap: true
                            }
                        },
                        {
                            loader: "sass-loader",
                            options: {
                                sourceMap: true
                            }
                        }
                    ]
                })
            }
        ]
    },
    plugins: [
        new CleanWebpackPlugin([buildPath],
            {
                root: webpackRoot,
                verbose: true,
                dry: false
            }),
        new CopyWebpackPlugin([
            {
                from: {
                    glob: "./Content/assets/**/*",
                    dot: false
                },
                to: "static",
                flatten: true
            }
        ]),
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery",
            'window.jQuery': "jquery",
            Popper: ["popper.js", "default"]
        }),
        new ExtractTextPlugin({
            filename: "identity.min.css"
        })
    ],
    optimization: {
        splitChunks: {
            cacheGroups: {
                default: false,
                commons: {
                    test: /[\\/]node_modules[\\/]/,
                    name: "vendor",
                    chunks: "all"
                }
            }
        }
    },
    devtool: "source-map"
};
// ReSharper restore Es6Feature