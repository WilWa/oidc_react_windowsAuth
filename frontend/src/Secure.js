import React from 'react';
import Axios from 'axios';
import { useHistory } from 'react-router-dom';
import AuthProvider from './authProvider';

function Unsecure() {
  let history = useHistory();
  const { signoutRedirect, user } = React.useContext(AuthProvider.context);

  const routeChange = () => {
    let path = `unsecure`;
    history.push(path);
  };

  const [error, setError] = React.useState({});
  const [data, setData] = React.useState([]);

  React.useEffect(() => {
    const getData = async () => {
      try {
        const response = await Axios.get(
          'https://localhost:44373/weatherforecast',
          {
            headers: {
              Authorization: `${user.token_type} ${user.access_token}`,
            },
          }
        );
        setData(response.data);
        console.log(response.data);
      } catch (err) {
        setError(err);
      }
    };
    getData();
  }, [setData, setError, user.access_token, user.token_type]);

  return (
    <div>
      <div>Secure, logged in as {user.profile.name}</div>
      {data.length <= 0 ? (
        <div>Loading data..</div>
      ) : (
        <div>
          <div>Data:</div>
          <table>
            <tr>
              <th>Date</th>
              <th>Celsius</th>
              <th>Farenheit</th>
              <th>Summary</th>
            </tr>
            {data.map((w) => (
              <tr>
                <td>{w.date}</td>
                <td>{w.temperatureC}</td>
                <td>{w.temperatureF}</td>
                <td>{w.summary}</td>
              </tr>
            ))}
          </table>
        </div>
      )}
      {!!error.message ? <div>Error: {error.message}</div> : ''}
      <div>
        <button onClick={routeChange}>Unsecure</button>
        <button onClick={signoutRedirect}>Logout</button>
      </div>
    </div>
  );
}

export default Unsecure;
