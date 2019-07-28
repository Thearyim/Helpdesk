const webpack = require('webpack');
const { resolve } = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {

    entry: [
        'react-hot-loader/patch',
        'webpack-dev-server/client?http://localhost:8080',
        'webpack/hot/only-dev-server',
        resolve(__dirname, "src", "index.jsx")
    ],

    output: {
        filename: 'app.bundle.js',
        path: resolve(__dirname, 'build'),
        publicPath: '/'
    },

    resolve: {
        alias: {
            SiteCss: resolve(__dirname, 'src/css/site.css'),
            SiteImages: resolve(__dirname, 'src/images'),
            SiteJs: resolve(__dirname, 'src/js/site.js'),
            SiteState: resolve(__dirname, 'src/js/state.js'),
            SiteStateActions: resolve(__dirname, 'src/js/state.actions.js'),
        },
        extensions: ['.js', '.jsx']
    },

    devtool: '#source-map',

    devServer: {
        hot: true,
        contentBase: resolve(__dirname, 'build'),
        publicPath: '/'
    },

    module: {
        rules: [
            {
                test: /\.jsx?$/,
                loader: "babel-loader",
                exclude: /node_modules/,
                options: {
                    presets: [
                        ["es2015", { "modules": false }],
                        "react",
                    ],
                    plugins: [
                        "react-hot-loader/babel"
                    ]
                }
            },
            {
                test: /\.ts?$/,
                use: 'ts-loader',
                exclude: /node_modules/
            },
            {
                test: /\.css$/,
                loader: 'style-loader!css-loader'
            },
            {
                test: /\.(png|gif|jp(e*)g|svg)$/,
                use: {
                    loader: 'url-loader',
                    options: {
                        limit: 8000,
                        name: 'images/[name].[ext]'
                    }
                }
            }
        ]
    },

    plugins: [
        new webpack.HotModuleReplacementPlugin(),
        new webpack.NamedModulesPlugin(),
        new HtmlWebpackPlugin({
            template: 'template.ejs',
            appMountId: 'react-app-root',
            title: 'helpdesk-web-application',
            filename: resolve(__dirname, "build", "index.html"),
        }),
    ]
};
