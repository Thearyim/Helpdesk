const webpack = require('webpack');
const { resolve } = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const dotenv = require('dotenv').config({path: __dirname + '/.env'})

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
            SiteImages: resolve(__dirname, 'src/images'),
            SiteState: resolve(__dirname, 'src/js/State.js'),
            SiteStateActions: resolve(__dirname, 'src/js/StateActions.js'),
            SiteSession: resolve(__dirname, 'src/js/Session.js'),
            SessionClient: resolve(__dirname, 'src/js/SessionClient.js'),
            TicketClient: resolve(__dirname, 'src/js/TicketClient.js')
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
                test: /\.(js|jsx)$/,
                loader: "babel-loader",
                exclude: /node_modules/,
                options: {
                    presets: [
                        "@babel/preset-env",
                        "@babel/preset-react"
                    ],
                    plugins: [
                        "@babel/plugin-proposal-class-properties",
                        "@babel/plugin-proposal-object-rest-spread",
                        "@babel/plugin-transform-runtime",
                        "react-hot-loader/babel"
                    ]
                }
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
            },
            {
                test: /\.html$/,
                use: [
                    {
                        loader: "html-loader"
                    }
                ]
            }
        ]
    },

    plugins: [
        new webpack.DefinePlugin({
            "process.env": dotenv.parsed
        }),
        new webpack.HotModuleReplacementPlugin(),
        new webpack.NamedModulesPlugin(),
        new HtmlWebpackPlugin({
            template: 'template.ejs',
            appMountId: 'react-app-root',
            title: 'Helpdesk',
            filename: resolve(__dirname, "build", "index.html"),
        }),
    ]
};
