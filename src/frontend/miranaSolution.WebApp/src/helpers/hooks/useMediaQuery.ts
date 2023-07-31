import {useEffect, useState} from 'react';

const useMediaQuery = (query: string) => {
    const getMatches = (): boolean => {
        if (typeof window === undefined) {
            return false;
        }

        return window.matchMedia(query).matches;
    }

    const [matches, setMatches] = useState<boolean>(getMatches());

    const handleChange = () => {
        setMatches(getMatches());
    }

    useEffect(() => {
        const mediaQueryList = window.matchMedia(query);

        if (Boolean(mediaQueryList.addListener)) {
            mediaQueryList.addListener(handleChange);
        } else {
            mediaQueryList.addEventListener("change", handleChange);
        }

        return () => {
            if (Boolean(mediaQueryList.removeListener)) {
                mediaQueryList.removeListener(handleChange);
            } else {
                mediaQueryList.removeEventListener("change", handleChange);
            }
        }
    });

    return matches as typeof matches;
};

export {useMediaQuery};
