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
        <div className="flex flex-col md:flex-row ml-[-30px]">
          <div className="w-[calc(calc(2*calc(100%/3))-30px)] ml-[30px]">
            <EditorRecommendation />
          </div>

          <div className="w-[calc(calc(100%/3)-30px)] ml-[30px]">
            <EditorRecommendation />
          </div>
        </div>
      </Section>
    </>
  );
};

export { Home };
