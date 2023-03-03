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

      <Section>
        <div className="flex flex-col md:flex-row ml-[-20px]">
          <div className="w-[calc(calc(2*calc(100%/3))-20px)] ml-[20px] min-h-[600px]">
            <EditorRecommendation />
          </div>

          <div className="w-[calc(calc(100%/3)-20px)] ml-[20px] min-h-[600px]">
            <CurrentlyReading />
          </div>
        </div>
      </Section>
    </>
  );
};

export { Home };
