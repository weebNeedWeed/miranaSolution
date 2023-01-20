import React, { useState } from "react";
import { GenreTag, Slider } from "../components";
import { BsBoxArrowInRight } from "react-icons/bs";
import { AnimatePresence, motion } from "framer-motion";

const motionVariants = {
	initial: { opacity: 0, y: 100 },
	animate: { opacity: 1, y: 0 },
	exit: { opacity: 0, y: -100 },
};

const Header = (): JSX.Element => {
	return (
		<div className="px-4 lg:px-16 py-4 bg-gradient flex items-center">
			<div className="w-full flex flex-col-reverse md:flex-row justify-start items-center min-h-[70vh]">
				<AnimatePresence mode="wait">
					<motion.div
						// add key here
						key={0}
						variants={motionVariants}
						initial="initial"
						animate="animate"
						exit="exit"
						className="w-full md:w-2/3 h-full flex flex-col justify-center items-start px-0 lg:px-20"
					>
						<motion.h2
							variants={motionVariants}
							className="uppercase font-bold text-3xl md:text-4xl lg:text-6xl mb-4 gradient-text"
						>
							{"ĐỆ NHẤT TẦN TRANH"}
						</motion.h2>
						<div className="flex flex-row w-full mb-3 justify-start items-center">
							<GenreTag genre="Bách hợp" className="mr-2" />
							<GenreTag genre="Khác" />
						</div>
						<p className="text-base text-deepKoamaru mb-4">
							Bố mẹ mất tích, để có khả năng tìm ra họ, bất đắc dĩ tôi phải làm
							người câm mười năm. Nhưng tại thời điểm này tôi lại bị bắt đi ở
							rể, mặc dù vợ xinh đẹp nhưng chưa bao giờ cho tôi sắc mặt tốt dù
							chỉ một ngày, cô ấy nói tôi là đồ bỏ đi! Và hôm nay, thời hạn mười
							năm kết thúc! Tần tranh Tôi sẽ lấy lại tất cả mọi thứ một lần
							nữa….
						</p>
						<button className="group flex flex-row justify-center items-center outline-none rounded-md border-2 border-solid border-deepKoamaru px-3 py-1 text-lg font-semibold font-sansPro text-deepKoamaru bg-[rgba(var(--color-old-rose),0.6)] transition-all hover:bg-[rgba(var(--color-old-rose),0.4)]">
							<p className="mr-1 group-hover:mr-2 transition-all">Đọc ngay</p>
							<span>
								<BsBoxArrowInRight />
							</span>
						</button>
					</motion.div>
				</AnimatePresence>

				<div className="w-full mb-4 p-2 md:w-1/3 flex flex-row items-center justify-end">
					<Slider />
				</div>
			</div>
		</div>
	);
};

export { Header };
