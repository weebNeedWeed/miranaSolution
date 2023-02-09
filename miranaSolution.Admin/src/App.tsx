import { Admin } from "react-admin";
import jsonServerProvider from "ra-data-json-server";
import "./App.css"

const dataProvider = jsonServerProvider("https://jsonplaceholder.typicode.com");

const App = (): JSX.Element => <Admin dataProvider={dataProvider} />;

export { App };
