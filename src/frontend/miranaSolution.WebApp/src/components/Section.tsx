import clsx from "clsx";
import React from "react";

type SectionProps = { children: React.ReactNode; className?: string };

const Section = ({children, className}: SectionProps): JSX.Element => {
    return (
        <div className={clsx("px-8 py-8 lg:px-16 w-full", className)}>
            <div className="w-[1140px] max-w-full mx-auto">
                {children}
            </div>
        </div>
    );
};

export {Section};
