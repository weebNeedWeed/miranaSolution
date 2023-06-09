ClassicEditor
    .create( document.querySelector( '#Content' ) )
    .catch( error => {
        console.error( error );
    } );