import React, { useState } from "react";

import { Navigation, Pagination, A11y, Autoplay } from "swiper";
import { Swiper, SwiperSlide } from "swiper/react";
import { EffectFade } from "swiper";
import SwiperClass from "swiper/types/swiper-class";

import "swiper/css";
import "swiper/css/navigation";
import "swiper/css/pagination";
import "swiper/css/effect-fade";

type SliderProps = {};
type SwiperChildSlideProps = {
	image: string;
};

const SwiperSlideChild = ({ image }: SwiperChildSlideProps): JSX.Element => {
	return (
		<div
			className={`w-[215px] h-[322px] bg-cover bg-center`}
			style={{
				backgroundImage: `url('${image}')`,
			}}
		></div>
	);
};

const Slider = (props: SliderProps): JSX.Element => {
	const handleOnSlideChange = (swiper: SwiperClass) => {
		console.log(swiper);
	};

	return (
		<div className="w-full">
			<Swiper
				modules={[Navigation, Autoplay, Pagination, A11y, EffectFade]}
				slidesPerView={1}
				navigation
				pagination={{ clickable: true }}
				onSlideChange={handleOnSlideChange}
				effect={"fade"}
				autoplay={{
					delay: 10000,
					disableOnInteraction: false,
				}}
			>
				<SwiperSlide className="flex flex-row items-center justify-center">
					<SwiperSlideChild image="https://static.cdnno.com/poster/can-cu-so-7/300.jpg?1650802893" />
				</SwiperSlide>
				<SwiperSlide className="flex flex-row items-center justify-center">
					<SwiperSlideChild image="https://static.cdnno.com/poster/can-cu-so-7/300.jpg?1650802893" />
				</SwiperSlide>
			</Swiper>
		</div>
	);
};

export { Slider };
