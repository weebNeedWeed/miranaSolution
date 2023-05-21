import {
  Box,
  BoxProps,
  Button,
  TextareaAutosize,
  TextField,
  Typography,
  Checkbox,
  Select,
  MenuItem
} from "@mui/material";
import { Link } from "react-router-dom";
import React, { ChangeEvent, useEffect, useState } from "react";
import { Formik, Form, Field, ErrorMessage, useFormik } from 'formik';
import { FormikHelpers } from "formik/dist/types";
import { styled } from "@mui/material/styles";
import { MuiFileInput } from "mui-file-input";
import { Author } from "../../models/Author";
import { useQuery } from "react-query";
import { booksApiClient } from "../../helpers/apis/BooksApiClient";
import { authorsApiClient } from "../../helpers/apis/AuthorsApiClient";
import { genresApiClient } from "../../helpers/apis/GenresApiClient";
import { Genre } from "../../models/Genre";

type FormObjValue = {
  name: string;
  shortDescription: string;
  longDescription: string;
  isRecommended: boolean;
  slug: string;
  authorId: number;
  thumbnailImage: File | undefined
};

type GenreCheckboxItem = {
  id: number;
  label: string;
  isChecked: boolean;
}

const initialValues: FormObjValue = {
  name: "",
  shortDescription: "",
  longDescription: "",
  isRecommended: false,
  slug: "",
  authorId: -1,
  thumbnailImage: undefined,
};

const FormGroup = styled(Box)<BoxProps>(() => ({
  marginTop: "1rem",
  width: "100%",
  display: "flex",
  flexDirection: "column"
}));

type UploadImageProps = {
  src: string
};
const UploadImage = styled("div")<UploadImageProps>(({src}) => ({
  width: "100%",
  marginTop: "1rem",
  height: "400px",
  backgroundImage: `url('${src}')`,
  backgroundRepeat: "no-repeat",
  backgroundPosition: "center",
  backgroundSize: "100%"
}));

const BooksCreatePage = (): JSX.Element => {
  const [ selectedFile, setSelectedFile ] = useState<File | null>(null);
  const [ preview, setPreview ] = useState<string>("");
  const [ authorList, setAuthorList ] = useState<Author[]>([]);
  const [ genreList, setGenreList ] = useState<Array<Genre>>([]);
  const [ genreCheckboxList, setGenreCheckboxList ] = useState<Array<GenreCheckboxItem>>([]);

  const {isLoading, isError, data, error} = useQuery({
    queryKey: [ 'authors' ],
    queryFn: () => authorsApiClient.GetAll(),
  });

  const genreResponse = useQuery({
    queryKey: [ 'genres' ],
    queryFn: () => genresApiClient.GetAll(),
    staleTime: Infinity
  });

  useEffect(() => {
    if (isLoading || isError || error || !data) {
      return;
    }

    setAuthorList(data);

  }, [ isLoading, isError, data, error ]);

  useEffect(() => {
    if (genreResponse.isLoading || genreResponse.isError || genreResponse.error || !genreResponse.data) {
      return;
    }

    setGenreList(genreResponse.data);

    setGenreCheckboxList(genreResponse.data.map((genre): GenreCheckboxItem => ({
      id: genre.id,
      label: genre.name,
      isChecked: false
    })));
  }, [ genreResponse.isLoading, genreResponse.isError, genreResponse.data, genreResponse.error ]);

  useEffect(() => {
    if (!selectedFile) {
      return;
    }

    const objUrl = URL.createObjectURL(selectedFile as File);
    setPreview(objUrl);

    return () => {
      URL.revokeObjectURL(objUrl);
    };
  }, [ selectedFile ]);

  const handleFormikSubmit = async (values: FormObjValue, {setSubmitting}: FormikHelpers<FormObjValue>) => {
    setTimeout(() => {
      setSubmitting(false);
    }, 400);

    const formData = new FormData();
    formData.append("name", values.name);
    formData.append("shortDescription", values.shortDescription);
    formData.append("longDescription", values.shortDescription);
    formData.append("isRecommended", Boolean(values.isRecommended).toString());
    formData.append("slug", values.slug);
    formData.append("authorId", values.authorId.toString());
    formData.append("thumnailImage", values.thumbnailImage as File);

    genreCheckboxList.forEach((genre, index) => {
      formData.append(`genres[${index}].id`, genre.id.toString());
      formData.append(`genres[${index}].label`, genre.label);
      formData.append(`genres[${index}].isChecked`, Boolean(genre.isChecked).toString());
    });

    const book = await booksApiClient.create(formData);
    if (!book) {
      alert("Error when creating a new book.");
    } else {
      alert("Create new book successfully");
    }
  };

  const handleGenreChange = (event: ChangeEvent<HTMLInputElement>, index: number) => {
    setGenreCheckboxList((prev) => {
      prev[index].isChecked = event.target.checked;
      return prev;
    });
  };

  return <>
    <Link to="/dashboard/books">
      <Button variant="contained" color="success">
        Back
      </Button>
    </Link>

    <Typography variant="h4" gutterBottom sx={{width: "100%", marginTop: "1rem"}}>
      Create new book
    </Typography>

    <Box sx={{width: "max(400px, 75vw)", marginBottom: "100px"}}>
      <Formik
        initialValues={initialValues}
        onSubmit={handleFormikSubmit}
      >
        {({values, isSubmitting, setFieldValue, handleChange}) => (
          <Form>
            <Box sx={{display: "flex", flexDirection: "row", marginLeft: "-20px"}}>
              <Box sx={{width: "calc(calc(100% / 2) - 20px)", marginLeft: "20px"}}>
                <Box
                  sx={{flexDirection: "column", display: "flex", width: "100%"}}
                >
                  <FormGroup>
                    <Typography variant="body1" display="block" gutterBottom>
                      Name
                    </Typography>
                    <TextField
                      name="name"
                      required
                      type="text"
                      value={values.name}
                      onChange={handleChange}
                    />
                  </FormGroup>

                  <FormGroup>
                    <Typography variant="body1" display="block" gutterBottom>
                      Short description
                    </Typography>
                    <TextareaAutosize style={{resize: "none"}} name="shortDescription"
                                      value={values.shortDescription}
                                      minRows={7}
                                      onChange={handleChange}/>
                  </FormGroup>

                  <FormGroup>
                    <Typography variant="body1" display="block" gutterBottom>
                      Long description
                    </Typography>
                    <TextareaAutosize style={{resize: "none"}} name="longDescription"
                                      value={values.longDescription}
                                      minRows={7}
                                      onChange={handleChange}/>
                  </FormGroup>

                  <FormGroup>
                    <Box sx={{display: "flex", flexDirection: "row", alignItems: "center"}}>
                      <Typography variant="body1" display="block">
                        Is recommended
                      </Typography>
                      <Checkbox name="isRecommended" value={values.isRecommended}
                                onChange={handleChange}/>
                    </Box>
                    <ErrorMessage name="isRecommended" component="div"/>
                  </FormGroup>

                  <FormGroup>
                    <Typography variant="body1" display="block" gutterBottom>
                      Slug
                    </Typography>
                    <TextField
                      name="slug"
                      required
                      type="text"
                      value={values.slug}
                      onChange={handleChange}
                    />
                  </FormGroup>

                  <FormGroup>
                    <Typography variant="body1" display="block" gutterBottom>
                      Author
                    </Typography>
                    {authorList && <Select required name="authorId"
                                           value={values.authorId}
                                           onChange={handleChange}>
                      <MenuItem value={-1}>Select author</MenuItem>
                      {authorList.map((author) => <MenuItem value={author.id} key={author.id}>{author.name}</MenuItem>)}
                    </Select>}
                    <ErrorMessage name="authorId" component="div"/>
                  </FormGroup>
                </Box>
              </Box>

              <Box sx={{width: "calc(calc(100% / 2) - 20px)", marginLeft: "20px"}}>
                <Box
                  sx={{flexDirection: "column", display: "flex", width: "100%"}}
                >
                  <Box sx={{display: "flex", flexWrap: "wrap"}}>
                    {genreList && genreList.map((genre, index) => <Box key={genre.id}>
                      <Box sx={{display: "flex", flexDirection: "row", alignItems: "center"}}>
                        <Typography variant="body1" display="block">
                          {genre.name}
                        </Typography>
                        <Checkbox value={genreCheckboxList[index].isChecked}
                                  onChange={(event) => {
                                    handleGenreChange(event, index);
                                  }}/>
                      </Box>
                    </Box>)}
                  </Box>

                  <FormGroup>
                    <Typography variant="body1" display="block" gutterBottom>
                      Thumbnail image
                    </Typography>
                    <MuiFileInput required={!Boolean(values.thumbnailImage)} value={values.thumbnailImage}
                                  onChange={(file) => {
                                    setFieldValue("thumbnailImage", file);
                                    setSelectedFile(file);
                                  }}/>
                    <ErrorMessage name="thumnailImage" component="div"/>

                    {preview && <UploadImage src={preview}/>}
                  </FormGroup>
                </Box>
              </Box>
            </Box>

            <Button sx={{marginTop: "1rem"}} type="submit" disabled={isSubmitting} variant="contained">Save</Button>
          </Form>
        )}
      </Formik>
    </Box>
  </>;
};

export { BooksCreatePage };