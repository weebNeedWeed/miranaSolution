import {useEffect} from "react";
import {HashLoader} from "react-spinners";
import {AnimatePresence, motion} from "framer-motion";

type LoadingProps = {
    show: boolean;
};

const Loading = ({show}: LoadingProps): JSX.Element => {
    useEffect(() => {
        if (show) {
            document.body.style.overflowY = "hidden";

            return () => {
                document.body.style.overflowY = "scroll";
            };
        }
    }, [show]);

    return (
        <AnimatePresence>
            {show && (
                <motion.div
                    exit={{opacity: 0}}
                    className="fixed top-0 left-0 w-[100vw] h-[100vh] bg-whiteChocolate z-[10000] flex items-center justify-center"
                >
                    <HashLoader color="#BA8880" size={100}/>
                </motion.div>
            )}
        </AnimatePresence>
    );
};

export {Loading};
