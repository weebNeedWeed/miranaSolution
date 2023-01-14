/** @type {import('tailwindcss').Config} */
module.exports = {
	content: ["./src/**/*.{js,ts,tsx,jsx}"],
	theme: {
		extend: {
			fontFamily: {
				sansPro: ["Source Sans Pro", "sans-serif"],
			},
			colors: {
				whiteChocolate: "rgb(var(--color-white-chocolate))",
				darkVanilla: "rgb(var(--color-dark-vanilla))",
				oldRose: "rgb(var(--color-old-rose))",
				deepKoamaru: "rgb(var(--color-deep-koamaru))",
				powderBlue: "rgb(var(--color-powder-blue))",
			},
		},
	},
	plugins: [],
};
