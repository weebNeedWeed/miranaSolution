import {useState} from "react";
import {GenreTag, Section, Slider} from "../components";
import {BsBoxArrowInRight} from "react-icons/bs";
import {AnimatePresence, motion} from "framer-motion";
import {useQuery} from "react-query";
import {slideApiHelper} from "../helpers/apis/SlideApiHelper";
import {Link} from "react-router-dom";

const motionVariants = {
    initial: {opacity: 0, y: 50},
    animate: {opacity: 1, y: 0},
    exit: {opacity: 0, y: -50},
};

const Header = (): JSX.Element => {
    const [slideIndex, setSlideIndex] = useState<number>(0);

    const {isLoading, error, data} = useQuery(
        "slides",
        () => slideApiHelper.getAllSlides(),
    );

    if (isLoading || error || !data) {
        return <></>;
    }

    const slides = data.slides;

    if (slides.length === 0) {
        return <></>
    }

    const currentSlide = slides![slideIndex];

    const genres = currentSlide.genres.split(",").map((elm) => elm.trim());

    const tags = genres.map((elm, index) => (
        <GenreTag genre={elm} key={index} className="mr-2"/>
    ));

    return (
        <Section className="bg-gradient flex items-center">
            <div className="w-full flex flex-col-reverse md:flex-row justify-start items-center md:min-h-[70vh]">
                <AnimatePresence mode="wait">
                    <motion.div
                        key={currentSlide!.id}
                        variants={motionVariants}
                        initial="initial"
                        animate="animate"
                        exit="exit"
                        className="w-full md:w-2/3 h-full flex flex-col justify-center items-start px-0 lg:px-12"
                    >
                        <motion.h2
                            variants={motionVariants}
                            className="uppercase font-bold text-3xl md:text-4xl lg:text-6xl mb-4 gradient-text"
                        >
                            {currentSlide.name}
                        </motion.h2>
                        <div className="flex flex-row w-full justify-start items-center flex-wrap">
                            {tags}
                        </div>
                        <p className="text-base text-deepKoamaru mb-4">
                            {currentSlide.shortDescription}
                        </p>
                        <Link
                            to={currentSlide.url}
                            className="group flex flex-row justify-center items-center outline-none rounded-md border-2 border-solid border-deepKoamaru px-3 py-1 text-lg font-semibold font-sansPro text-deepKoamaru bg-[rgba(var(--color-old-rose),0.6)] transition-all hover:bg-[rgba(var(--color-old-rose),0.4)]">
                            <p className="mr-1 group-hover:mr-2 transition-all">Đọc ngay</p>
                            <span>
                                <BsBoxArrowInRight/>
                            </span>
                        </Link>
                    </motion.div>
                </AnimatePresence>

                <div className="w-full sm:w-1/2 mb-4 p-2 md:w-1/3 flex flex-row items-center justify-end">
                    <Slider
                        images={slides!.map((elm) => elm.thumbnailImage)}
                        setSlideIndex={setSlideIndex}
                    />
                </div>
            </div>
        </Section>
    );
};

export {Header};
