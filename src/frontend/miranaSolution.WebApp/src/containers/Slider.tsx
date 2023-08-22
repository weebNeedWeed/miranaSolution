import {A11y, Autoplay, EffectCube} from "swiper";
import {Swiper, SwiperSlide} from "swiper/react";
import SwiperClass from "swiper/types/swiper-class";

import "swiper/css";
import "swiper/css/effect-cube";
import "swiper/css/effect-fade";
import {useBaseUrl} from "../helpers/hooks/useBaseUrl";

type SliderProps = {
    images: Array<string>;
    setSlideIndex: (index: number) => void;
};
type SwiperChildSlideProps = {
    image: string;
};

const SwiperSlideChild = ({image}: SwiperChildSlideProps): JSX.Element => {
    const baseUrl = useBaseUrl();
    return (
        <div
            className={`w-[150px] h-[224px] sm:w-[215px] sm:h-[322px] bg-cover bg-center`}
            style={{
                boxShadow: "rgba(0, 0, 0, 0.35) 0px 5px 15px",
                backgroundImage: `url('${baseUrl}/${image}')`,
            }}
        ></div>
    );
};

const Slider = (props: SliderProps): JSX.Element => {
    const handleOnSlideChange = (swiper: SwiperClass) => {
        props.setSlideIndex(swiper.activeIndex);
    };

    return (
        <div className="w-full">
            <Swiper
                modules={[Autoplay, A11y, EffectCube]}
                slidesPerView={1}
                onSlideChange={handleOnSlideChange}
                effect={"cube"}
                allowTouchMove={false}
                autoplay={{
                    delay: 5000,
                    disableOnInteraction: false,
                }}
            >
                {props.images.map((elm, index) => (
                    <SwiperSlide
                        key={index}
                        className="flex flex-row items-center justify-center"
                    >
                        <SwiperSlideChild image={elm}/>
                    </SwiperSlide>
                ))}
            </Swiper>
        </div>
    );
};

export {Slider};
