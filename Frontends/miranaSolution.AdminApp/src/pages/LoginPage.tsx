import { Container, Box, TextField, Typography, Button } from "@mui/material";
import { usersApiClient } from "../helpers/apis/UsersApiClient";
import React, { useState } from "react";
import jwt_decode, { JwtPayload } from "jwt-decode";
import { useNavigate } from "react-router-dom";

interface MyJwtPayload extends JwtPayload {
  roles: Array<string>;
}

const validateIsAdmin = (token: string): boolean => {
  const decodedData = jwt_decode<MyJwtPayload>(token);
  const roles = decodedData.roles;

  return roles.indexOf("Administrator") > -1;
};

const LoginPage = (): JSX.Element => {
  const navigate = useNavigate();

  const [ userName, setUserName ] = useState("");
  const [ password, setPassword ] = useState("");
  const [ error, setError ] = useState("");

  const handleTypeUserName = (event: React.ChangeEvent<HTMLInputElement>) => {
    setUserName(event.target.value);
  };
  const handleTypePassword = (event: React.ChangeEvent<HTMLInputElement>) => {
    setPassword(event.target.value);
  };

  const handleSubmitForm = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!password.trim() || !userName.trim()) {
      return;
    }

    const token = await usersApiClient.authenticate({userName, password});
    if (!token || !validateIsAdmin(token)) {
      setError("Invalid credentials");
      return;
    }

    navigate("/dashboard");
  };

  return (
    <Container maxWidth="xs">
      <Box
        onSubmit={handleSubmitForm}
        component="form"
        sx={{
          width: "100%",
          padding: "30px 20px 20px 20px",
          marginTop: "100px",
          border: "1px solid rgba(0,0,0,0.5)",
          borderRadius: "4px",
          boxShadow: "rgba(100, 100, 111, 0.2) 0px 7px 29px 0px",
        }}
      >
        <Typography
          sx={{
            fontWeight: "bold",
            textTransform: "uppercase",
            marginBottom: "10px",
            textAlign: "center",
          }}
          variant="h3"
        >
          login
        </Typography>

        <TextField
          fullWidth
          label="Username"
          value={userName}
          onChange={handleTypeUserName}
          error={Boolean(error)}
          helperText={error ?? null}
        />
        <TextField
          value={password}
          sx={{marginTop: "20px"}}
          fullWidth
          label="Password"
          onChange={handleTypePassword}
        />

        <Button
          variant="contained"
          type="submit"
          fullWidth
          sx={{marginTop: "20px"}}
        >
          Login
        </Button>
      </Box>
    </Container>
  );
};

export { LoginPage };
