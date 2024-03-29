import {Divider, Header, Section} from "../components";
import {CurrentlyReadingsSection, EditorRecommendation, MostReading, NewestChapters,} from "../containers";
import {useMediaQuery} from "../helpers/hooks/useMediaQuery";

const Home = (): JSX.Element => {
    const matches = useMediaQuery("(min-width: 768px)");

    return (
        <>
            {matches && (
                <>
                    <Header/>
                    <Divider/>
                </>
            )}
            <Section className="pt-0 md:pt-8">
                <div className="flex flex-col md:flex-row md:ml-[-20px]">
                    <div
                        className="w-full md:w-[calc(calc(2*calc(100%/3))-20px)] md:ml-[20px] mb-4 md:mb-0 md:min-h-[600px]">
                        <EditorRecommendation/>
                    </div>

                    <div className="w-full md:w-[calc(calc(100%/3)-20px)] md:ml-[20px] md:min-h-[600px]">
                        <CurrentlyReadingsSection/>
                    </div>
                </div>
            </Section>

            <Divider/>

            <Section>
                <NewestChapters/>
            </Section>

            <Divider/>

            <Section>
                <MostReading/>
            </Section>
        </>
    );
};

export {Home};
