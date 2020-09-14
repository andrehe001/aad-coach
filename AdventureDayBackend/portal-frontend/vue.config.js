module.exports = {
  devServer: {
    proxy: {
      "/api": {
        target: "https://localhost:44301",
        changeOrigin: true,
        logLevel: "debug"
      }
    }
  }
};