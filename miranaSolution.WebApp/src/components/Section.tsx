import React from "react";

type SectionProps = { children: React.ReactNode };

const Section = ({ children }: SectionProps): JSX.Element => {
	return <div className="px-8 py-8 lg:px-16 w-full">{children}</div>;
};

export { Section };
