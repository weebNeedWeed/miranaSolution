import React from "react";
import { motion, AnimatePresence } from "framer-motion";

const data = [
	{
		id: 0,
		name: "someone",
	},
	{
		id: 1,
		name: "someone 2",
	},
	{
		id: 2,
		name: "someone 3",
	},
];

const Test = (): JSX.Element => {
	const [index, setIndex] = React.useState(1);

	const renderData = data.find((elm) => elm.id === index);

	return (
		<div className="p-32">
			<AnimatePresence mode="wait">
				<motion.div
					key={Math.random() * 10}
					initial={{ opacity: 0, y: 100 }}
					animate={{ opacity: 1, y: 0 }}
					exit={{ opacity: 0, y: -100 }}
					className="w-32 h-32 bg-slate-500"
				>
					<p>{renderData?.name}</p>
				</motion.div>
			</AnimatePresence>

			<button
				onClick={() => {
					setIndex((prev) => {
						return (prev + 1) % 3;
					});
				}}
			>
				Click to change
			</button>
		</div>
	);
};

export { Test };
