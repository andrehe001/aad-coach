module.exports = {
  devServer: {
    proxy: {
      "/api": {
        target: "https://localhost:44315",
        changeOrigin: true,
        logLevel: "debug"
      }
    }
  }
};