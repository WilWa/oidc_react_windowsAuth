import React from 'react';
import { useHistory } from 'react-router-dom';

function Unsecure() {
  let history = useHistory();

  const routeChange = () => {
    let path = `secure`;
    history.push(path);
  };

  return (
    <div>
      <div>Unsecure</div>
      <div>
        <button onClick={routeChange}>Secure</button>
      </div>
    </div>
  );
}

export default Unsecure;
