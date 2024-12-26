/** @type {import('tailwindcss').Config} */

module.exports = {
  content: [
    "./Views/**/*.cshtml", // Pour les fichiers Razor d'ASP.NET
    "./wwwroot/**/*.html", // Pour les fichiers HTML
    "./wwwroot/js/**/*.js", // Scripts JavaScript
    "./node_modules/flowbite/**/*.js",
  ],
  theme: {
    extend: {},
  },
  plugins: [require("flowbite/plugin")],
};
