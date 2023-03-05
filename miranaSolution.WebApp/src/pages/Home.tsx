import { Divider, Section } from "../components";
import { CurrentlyReading, EditorRecommendation } from "../containers";
import { useMediaQuery } from "../helpers/hooks/useMediaQuery";
import { Header } from "../layouts";

const Home = (): JSX.Element => {
  const matches = useMediaQuery("(min-width: 768px)");

  return (
    <>
      {matches && (
        <>
          <Header />
          <Divider />
        </>
      )}
      <Section className="pt-0 md:pt-8">
        <div className="flex flex-col md:flex-row md:ml-[-20px]">
          <div className="w-full md:w-[calc(calc(2*calc(100%/3))-20px)] md:ml-[20px] mb-4 md:mb-0 md:min-h-[600px]">
            <EditorRecommendation />
          </div>

          <div className="w-full md:w-[calc(calc(100%/3)-20px)] md:ml-[20px] md:min-h-[600px]">
            <CurrentlyReading />
          </div>
        </div>
      </Section>
      <Divider />a
    </>
  );
};

export { Home };
