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

			keyframes: {
				slideLeft: {
					"0%": {
						transform: "translateX(0)",
					},
					"100%": {
						transform: "translateX(-100%)",
					},
				},
			},

			animation: {
				slideLeft:
					"slideLeft 0.5s cubic-bezier(0.250, 0.460, 0.450, 0.940) both",
			},

			backgroundImage: {
				gradient: "var(--bg-gradient)",
			},
		},
	},
	plugins: [],
};
