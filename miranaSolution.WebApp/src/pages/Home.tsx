import { Divider } from "../components";
import { EditorRecommendationWithCurrentlyReading, Header } from "../layouts";

const Home = (): JSX.Element => {
	return (
		<>
			<Header />

			<Divider />

			<EditorRecommendationWithCurrentlyReading />
		</>
	);
};

export { Home };
