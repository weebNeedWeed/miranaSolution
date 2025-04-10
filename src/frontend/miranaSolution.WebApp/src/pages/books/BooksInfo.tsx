import { Avatar, Pager, Rating, Section } from "../../components";
import {
  Link,
  useNavigate,
  useParams,
  useSearchParams,
} from "react-router-dom";
import React, { useEffect, useMemo, useState } from "react";
import { bookApiHelper } from "../../helpers/apis/BookApiHelper";
import { Book } from "../../helpers/models/catalog/books/Book";
import clsx from "clsx";
import { GetAllChaptersResponse } from "../../helpers/models/catalog/books/GetAllChaptersResponse";
import { AiFillRead, AiFillStar } from "react-icons/ai";
import { BsFillBookmarkCheckFill, BsFillBookmarkFill } from "react-icons/bs";
import { BiTime, BiUpArrowAlt } from "react-icons/bi";
import { GetBookBySlugRequest } from "../../helpers/models/catalog/books/GetBookBySlugRequest";
import authorIcon from "./../../assets/author.jpg";
import { Bookmark } from "../../helpers/models/catalog/bookmark/Bookmark";
import { bookmarkApiHelper } from "../../helpers/apis/BookmarkApiHelper";
import { useAccessToken } from "../../helpers/hooks/useAccessToken";
import { useSystemContext } from "../../contexts/SystemContext";
import { useAuthenticationContext } from "../../contexts/AuthenticationContext";
import { ToastVariant } from "../../components/Toast";
import {
  CommentsSection,
  RatingDialog,
  RatingsSection,
} from "../../containers";
import { useQuery } from "react-query";
import { authorApiHelper } from "../../helpers/apis/AuthorApiHelper";
import { BookRating } from "../../helpers/models/catalog/books/BookRating";
import { BookUpvote } from "../../helpers/models/catalog/books/BookUpvote";
import { bookUpvoteApiHelper } from "../../helpers/apis/BookUpvoteApiHelper";
import { IoIosArrowForward } from "react-icons/io";
import { MobileChapterList } from "../../containers/MobileChapterList";
import { currentlyReadingApiHelper } from "../../helpers/apis/CurrentlyReadingApiHelper";
import { useLocalStorage } from "../../helpers/hooks/useLocalStorage";
import { CurrentlyReading } from "../../helpers/models/catalog/currentlyReading/CurrentlyReading";
import { useBaseUrl } from "../../helpers/hooks/useBaseUrl";
import { Helmet } from "react-helmet";
import { GetRatingsOverviewResponse } from "../../helpers/models/catalog/books/GetRatingsOverviewResponse";

type MobileTabsSectionProps = {
  getChaptersResponse: GetAllChaptersResponse;
  book: Book;
  ratingOverview?: GetRatingsOverviewResponse;
};
const MobileTabSection = (props: MobileTabsSectionProps) => {
  const { getChaptersResponse, book, ratingOverview } = props;
  const [open, setOpen] = useState(false);

  return (
    <>
      <button
        onClick={() => setOpen(!open)}
        className="text-base font-semibold bg-oldRose px-6 flex-row flex justify-between items-center py-1"
      >
        Danh sách chương ({getChaptersResponse.totalChapters} chương)
        <span className="text-base">
          <IoIosArrowForward />
        </span>
      </button>

      <div className="flex flex-col w-full mt-7">
        <h4 className="text-base font-semibold bg-oldRose px-6 py-1">
          Giới thiệu
        </h4>

        <div className="px-6 pt-1">{book.longDescription}</div>
      </div>

      <div className="flex flex-col w-full mt-6">
        <h4 className="text-base font-semibold bg-oldRose px-6 py-1">
          Tác giả
        </h4>

        <div className="px-4 py-2">
          <AuthorBooks
            book={book}
            isMobile
            authorId={book.authorId}
            authorName={book.authorName}
          />
        </div>
      </div>

      <div className="flex flex-col w-full mt-6">
        <h4 className="text-base font-semibold bg-oldRose px-6 py-1">
          Đánh giá
        </h4>

        <div className="pl-14 pb-2 pt-1">
          {ratingOverview && (
            <div className="flex flex-col mt-1 w-full">
              {Array.from(new Array(5)).map((_, index) => (
                <div
                  key={index}
                  className="flex flex-row gap-x-2 items-center text-base font-normal"
                >
                  <Rating value={5 - index} width={16} spacing={2} />

                  <span className="text-sm">
                    {ratingOverview.ratingsByStar[5 - index]} đánh giá
                  </span>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>

      <MobileChapterList
        book={book}
        open={open}
        handleClose={() => setOpen(false)}
      />
    </>
  );
};

type AuthorBooks = {
  authorId: number;
  authorName: string;
  isMobile?: boolean;
  book: Book;
};
const AuthorBooks = (props: AuthorBooks): JSX.Element => {
  const { authorId, authorName, book } = props;
  const { data: booksData } = useQuery(["author", authorId], () =>
    authorApiHelper.getAllBooksByAuthorId(authorId)
  );

  const isMobile = props.isMobile ?? false;

  if (isMobile) {
    return (
      <div className="flex flex-row bg-transparent w-full">
        <div className="flex flex-col items-center grow-0 w-1/4">
          <Avatar className="w-10 h-10" imageUrl={authorIcon} />
          <span className="text-center w-full text-sm font-semibold mt-1 break-words">
            {authorName}
          </span>
        </div>

        <div className="flex flex-col grow">
          <span className="text-sm font-semibold">Truyện cùng tác giả:</span>

          <ul className="list-disc ml-5">
            {booksData && (
              <div className="flex flex-col w-full justify-start items-start text-base font-normal mt-1">
                {booksData
                  .filter((book) => book.id !== book.id)
                  .map((book) => (
                    <li key={book.id}>
                      <Link
                        to={`/books/${book.slug}`}
                        className="w-full line-clamp-1 text-left border-b-[2px]"
                      >
                        {book.name}
                      </Link>
                    </li>
                  ))}
              </div>
            )}
          </ul>
        </div>
      </div>
    );
  }

  return (
    <div className="flex flex-col bg-oldRose rounded-md py-6 min-w-[300px] min-h-[500px] h-full justify-start items-start">
      <div className="flex flex-row w-full justify-center">
        <Avatar className="w-24 h-24" imageUrl={authorIcon} />
      </div>
      <div className="text-center w-full text-base font-semibold mt-3 border-[rgba(0,0,0,0.2)] border-solid border-b-[1px]">
        {authorName}
      </div>

      <span className="text-center w-full text-base font-semibold mt-4">
        Truyện cùng tác giả
      </span>

      {booksData && (
        <div className="flex flex-col w-full justify-center items-center text-base font-normal mt-1">
          {booksData
            .filter((x) => x.id !== book.id)
            .map((book) => (
              <Link
                key={book.id}
                to={`/books/${book.slug}`}
                className="w-full text-center line-clamp-1 border-b-[1px] border-[rgba(0,0,0,0.2)] border-solid mb-2"
              >
                {book.name}
              </Link>
            ))}
        </div>
      )}
    </div>
  );
};

type ChapterListProps = {
  [T in keyof GetAllChaptersResponse]: GetAllChaptersResponse[T];
};
const ChapterList = (props: ChapterListProps): JSX.Element => {
  const { pageSize, pageIndex, totalPages, chapters } = props;

  const { slug } = useParams<string>();

  const renderArray: JSX.Element[][] = [];
  const sizeOfRow = 3;

  chapters.forEach((chapter, index) => {
    if (index % sizeOfRow === 0) {
      renderArray.push([]);
    }
    const actualIndex = Math.floor(index / sizeOfRow);
    renderArray[actualIndex].push(
      <Link
        key={index}
        to={`/books/${slug}/chapters/${chapter.index}`}
        className="w-1/3 pr-6 line-clamp-1"
      >
        Chương {chapter.index}: {chapter.name}
      </Link>
    );
  });

  return (
    <>
      <ul className="flex flex-col">
        {renderArray.map((sub, index) => (
          <li
            key={index}
            className="py-3 flex flex-row border-b-[1px] border-[rgba(48,54,89,0.4)]"
          >
            {sub}
          </li>
        ))}
      </ul>

      <div className="mt-auto">
        <Pager
          pageIndex={pageIndex}
          pageSize={pageSize}
          totalPages={totalPages}
        />
      </div>
    </>
  );
};

type TabsSectionProps = {
  getChaptersResponse: GetAllChaptersResponse;
  book: Book;
  ratingOverview?: GetRatingsOverviewResponse;
};
const TabsSection = ({
  getChaptersResponse,
  book,
  ratingOverview,
}: TabsSectionProps): JSX.Element => {
  const [tabIndex, setTabIndex] = useState<1 | 2 | 3>(1);

  const isTabIndexActive = (index: number) => {
    return index === tabIndex;
  };

  const getActiveTabButtonClassName = (tabIndex: number) => {
    return clsx(
      isTabIndexActive(tabIndex)
        ? "border-deepKoamaru"
        : "text-[rgba(48,54,89,0.4)]",
      "px-6 py-2 text-lg border-b-[1px] font-semibold border-solid"
    );
  };

  return (
    <div className="flex flex-col w-full">
      <div className="w-full flex flex-row border-[rgba(48,54,89,0.4)] border-b-[1px] box-border">
        <button
          onClick={() => setTabIndex(1)}
          className={getActiveTabButtonClassName(1)}
        >
          Thông tin chi tiết
        </button>

        <button
          onClick={() => setTabIndex(2)}
          className={getActiveTabButtonClassName(2)}
        >
          Danh sách chương ({getChaptersResponse.totalChapters} chương)
        </button>

        <button
          onClick={() => setTabIndex(3)}
          className={getActiveTabButtonClassName(3)}
        >
          Đánh giá
        </button>
      </div>

      <div className="w-full min-h-[650px] px-8 py-8 flex flex-col h-full">
        <div
          className={clsx(isTabIndexActive(1) ? "block" : "hidden", "w-full")}
        >
          <div className="flex flex-row w-full">
            <div className="mr-4 w-full flex flex-col">
              {book.longDescription}
            </div>

            <AuthorBooks
              book={book}
              authorId={book.authorId}
              authorName={book.authorName}
            />
          </div>
        </div>

        <div
          className={clsx(
            isTabIndexActive(2) ? "flex" : "hidden",
            "flex-col w-full h-full grow"
          )}
        >
          <ChapterList
            chapters={getChaptersResponse.chapters}
            pageIndex={getChaptersResponse.pageIndex}
            pageSize={getChaptersResponse.pageSize}
            totalPages={getChaptersResponse.totalPages}
            totalChapters={getChaptersResponse.totalChapters}
          />
        </div>

        <div
          className={clsx(isTabIndexActive(3) ? "block" : "hidden", "w-full")}
        >
          <RatingsSection book={book} ratingOverview={ratingOverview} />
        </div>
      </div>
    </div>
  );
};

const BooksInfo = (): JSX.Element => {
  const { slug } = useParams<string>();
  const navigate = useNavigate();
  const [getBookResponse, setBookResponse] = useState<GetBookBySlugRequest>();
  const [getAllChaptersResponse, setResponse] =
    useState<GetAllChaptersResponse>();
  const [bookmark, setBookmark] = useState<Bookmark>();
  const [searchParams] = useSearchParams();
  const [accessToken, setAccessToken] = useAccessToken();
  const systemContext = useSystemContext();
  const authContext = useAuthenticationContext();
  const [ratingOverview, setRatingOverview] =
    useState<GetRatingsOverviewResponse>();
  const [upvote, setUpvote] = useState<BookUpvote>();
  const [startChapter, setStartChapter] = useState(1);

  const [offlineReadingBooks, setOfflineReadingBooks] = useLocalStorage<
    CurrentlyReading[]
  >("readings", []);

  const [openRatingDialog, setOpenRatingDialog] = useState(false);

  const pageIndex = parseInt(searchParams.get("pageIndex") ?? "1", 10);
  const pageSize = parseInt(searchParams.get("pageSize") ?? "30", 10);

  const baseUrl = useBaseUrl();

  useEffect(() => {
    (async () => {
      try {
        const result = await bookApiHelper.getBookBySlug(slug!);
        setBookResponse(result);

        const getOverviewResponse = await bookApiHelper.getRatingsOverview(
          result.book.id
        );
        setRatingOverview(getOverviewResponse);

        const chaptersResult = await bookApiHelper.getAllChapters({
          bookId: result.book.id,
          pageIndex,
          pageSize,
        });

        setResponse(chaptersResult);
        let start = 1;

        if (authContext.state.isLoggedIn) {
          const bookmarks = await bookmarkApiHelper.getAllBookmark(
            accessToken,
            result.book.id
          );
          setBookmark(bookmarks[0]);

          let upvotes = await bookUpvoteApiHelper.getAllUpvotes(
            result.book.id,
            authContext.state.user.id
          );
          setUpvote(upvotes[0]);

          const getCurrReadingBooks =
            await currentlyReadingApiHelper.getCurrentlyReadingBooks(
              accessToken,
              result.book.id
            );
          if (getCurrReadingBooks.currentlyReadings.length > 0) {
            start = getCurrReadingBooks.currentlyReadings[0].chapterIndex;
          }
        } else {
          const currentChapter = offlineReadingBooks.find(
            (elm) => elm.bookId === result.book.id
          );
          if (currentChapter) {
            start = currentChapter.chapterIndex;
          }
        }

        setStartChapter(start);
      } catch (error: any) {
        navigate("/404");
      }
    })();
  }, [pageIndex, pageSize, authContext.state.isLoggedIn, slug]);

  if (!getBookResponse) {
    return <></>;
  }

  const book = getBookResponse.book;

  const handleCreateBookmark = async () => {
    if (!authContext.state.isLoggedIn) {
      systemContext.dispatch({
        type: "addToast",
        payload: {
          variant: ToastVariant.Warning,
          title: "Vui lòng đăng nhập để đánh dấu.",
        },
      });
      return;
    }

    const bookId = book.id;
    if (bookmark) {
      await bookmarkApiHelper.deleteBookmark(accessToken, bookId);
      systemContext.dispatch({
        type: "addToast",
        payload: {
          variant: ToastVariant.Success,
          title: "Huỷ đánh dấu thành công.",
        },
      });
      setBookmark(undefined);
      return;
    }

    const _bookmark = await bookmarkApiHelper.createBookmark(accessToken, {
      bookId,
      chapterIndex: 1,
    });
    systemContext.dispatch({
      type: "addToast",
      payload: {
        variant: ToastVariant.Success,
        title: "Đánh dấu thành công.",
      },
    });
    setBookmark(_bookmark);
  };

  const handleCreateUpvote = async () => {
    if (!authContext.state.isLoggedIn) {
      systemContext.dispatch({
        type: "addToast",
        payload: {
          variant: ToastVariant.Warning,
          title: "Vui lòng đăng nhập để đề cử.",
        },
      });
      return;
    }

    const bookId = book.id;
    if (upvote) {
      await bookUpvoteApiHelper.deleteUpvote(accessToken, bookId);
      systemContext.dispatch({
        type: "addToast",
        payload: {
          variant: ToastVariant.Success,
          title: "Huỷ đề cử thành công.",
        },
      });
      setUpvote(undefined);
      return;
    }

    const _upvote = await bookUpvoteApiHelper.createUpvote(accessToken, bookId);
    systemContext.dispatch({
      type: "addToast",
      payload: {
        variant: ToastVariant.Success,
        title: "Đề cử thành công.",
      },
    });
    setUpvote(_upvote);
  };

  return (
    <Section className="text-deepKoamaru pt-0 md:pt-8">
      <Helmet>
        <title>{book.name} | Mirana Readers</title>
      </Helmet>

      <div className="w-full bg-[rgba(255,255,255,0.8)] rounded-md shadow-sm shadow-slate-500 flex flex-col">
        <div className="w-full flex flex-row items-stretch px-6 pt-6 pb-0 md:pb-6">
          <span
            className="w-24 h-auto md:w-40 aspect-[3/4] shrink-0 block rounded-md bg-cover bg-center bg-no-repeat drop-shadow-md"
            style={{
              backgroundImage: `url('${baseUrl}${book.thumbnailImage}')`,
            }}
          ></span>

          <div className="ml-2 md:ml-4 grow w-full flex flex-col">
            <span className="text-lg font-extrabold capitalize w-full line-clamp-1">
              {book.name}
            </span>

            <div className="flex flex-row mt-1 md:mt-2 gap-x-1 md:gap-x-2 flex-wrap gap-y-1">
              <span className="text-xs py-1 px-3 rounded-3xl bg-oldRose text-white">
                {book.authorName}
              </span>

              <span className="text-xs py-1 px-3 rounded-3xl bg-darkVanilla text-white">
                {book.isDone ? "Hoàn thành" : "Chưa hoàn thành"}
              </span>

              <div className="md:flex flex-row gap-x-2 hidden">
                {book.genres.map((genre, index) => (
                  <span
                    key={index}
                    className="text-xs py-1 px-3 rounded-3xl bg-darkVanilla text-white"
                  >
                    {genre}
                  </span>
                ))}
              </div>
            </div>

            <div className="md:flex hidden flex-row justify-start items-center mt-2 gap-x-8">
              <div className="flex flex-col items-start">
                <span className="font-semibold text-xl">
                  {getAllChaptersResponse?.totalChapters}
                </span>

                <span className="font-normal text-base">Chương</span>
              </div>

              <div className="flex flex-col items-start">
                <span className="font-semibold text-xl">{book.viewCount}</span>

                <span className="font-normal text-base">Lượt đọc</span>
              </div>

              <div className="flex flex-col items-start">
                <span className="font-semibold text-xl">
                  {getBookResponse.totalBookmarks}
                </span>

                <span className="font-normal text-base">Cất giữ</span>
              </div>

              <div className="flex flex-col items-start">
                <span className="font-semibold text-xl">
                  {getBookResponse.totalUpvotes}
                </span>

                <span className="font-normal text-base">Đề cử</span>
              </div>
            </div>

            <div className="mt-1 md:mt-2 w-full flex flex-row gap-x-1">
              <Rating value={ratingOverview?.avg ?? 0} />
              <span className="text-base text-deepKoamaru">
                <span className="font-semibold">
                  {ratingOverview?.avg ?? 0}
                </span>
                <span className="md:inline-block hidden">
                  /5 ({ratingOverview?.totalRatings ?? 0} đánh giá)
                </span>
              </span>
            </div>

            <div className="mt-auto hidden flex-row gap-x-4 md:flex">
              <Link
                to={`/books/${book.slug}/chapters/${startChapter}`}
                className="item rounded bg-deepKoamaru py-1.5 px-3 mt-4 text-white font-semibold flex justify-center items-center gap-x-2 text-sm"
              >
                <AiFillRead /> Đọc truyện
              </Link>

              <button
                onClick={() => setOpenRatingDialog(!openRatingDialog)}
                className="item rounded bg-transparent py-1.5 px-3 mt-4 text-deepKoamaru font-semibold flex justify-center items-center gap-x-2 text-sm border-deepKoamaru border-2"
              >
                <AiFillStar /> Đánh giá
              </button>

              {!bookmark ? (
                <button
                  onClick={handleCreateBookmark}
                  className="item rounded bg-transparent py-1.5 px-3 mt-4 text-deepKoamaru font-semibold flex justify-center items-center gap-x-2 text-sm border-deepKoamaru border-2"
                >
                  <BsFillBookmarkFill /> Đánh dấu
                </button>
              ) : (
                <button
                  onClick={handleCreateBookmark}
                  className="item rounded bg-deepKoamaru py-1.5 px-3 mt-4 text-white font-semibold flex justify-center items-center gap-x-2 text-sm"
                >
                  <BsFillBookmarkCheckFill /> Đã đánh dấu
                </button>
              )}

              {!upvote ? (
                <button
                  onClick={handleCreateUpvote}
                  className="item rounded bg-transparent py-1.5 px-3 mt-4 text-deepKoamaru font-semibold flex justify-center items-center gap-x-2 text-sm border-deepKoamaru border-2"
                >
                  <BiUpArrowAlt /> Đề cử
                </button>
              ) : (
                <button
                  onClick={handleCreateUpvote}
                  className="item rounded bg-deepKoamaru py-1.5 px-3 mt-4 text-white font-semibold flex justify-center items-center gap-x-2 text-sm border-deepKoamaru border-2"
                >
                  <BiUpArrowAlt /> Đã đề cử
                </button>
              )}
            </div>
          </div>
        </div>

        <div className="w-full px-6 md:hidden flex flex-col mt-2 gap-y-2">
          <div className="md:hidden flex flex-row justify-around items-center">
            <div className="flex flex-col items-start">
              <span className="font-semibold text-lg">
                {getAllChaptersResponse?.totalChapters}
              </span>

              <span className="font-normal text-sm">Chương</span>
            </div>

            <div className="flex flex-col items-start">
              <span className="font-semibold text-lg">{book.viewCount}</span>

              <span className="font-normal text-sm">Lượt đọc</span>
            </div>

            <div className="flex flex-col items-start">
              <span className="font-semibold text-lg">
                {getBookResponse.totalBookmarks}
              </span>

              <span className="font-normal text-sm">Cất giữ</span>
            </div>

            <div className="flex flex-col items-start">
              <span className="font-semibold text-lg">
                {getBookResponse.totalUpvotes}
              </span>

              <span className="font-normal text-sm">Đề cử</span>
            </div>
          </div>

          <div className="mt-auto flex flex-row gap-y-2 w-full flex-wrap">
            <div className="flex flex-row gap-x-2 w-full">
              <Link
                to={`/books/${book.slug}/chapters/${startChapter}`}
                className="item rounded bg-deepKoamaru py-1.5 px-2 text-white font-semibold flex justify-center items-center gap-x-1 text-sm w-1/2"
              >
                <AiFillRead /> Đọc truyện
              </Link>

              <button
                onClick={() => setOpenRatingDialog(!openRatingDialog)}
                className="item rounded bg-transparent py-1.5 px-2 text-deepKoamaru font-semibold flex justify-center items-center gap-x-2 text-sm border-deepKoamaru border-2 w-1/2"
              >
                <AiFillStar /> Đánh giá
              </button>
            </div>

            <div className="flex flex-row gap-x-2 w-full">
              {!bookmark ? (
                <button
                  onClick={handleCreateBookmark}
                  className="item rounded bg-transparent py-1.5 px-2 text-deepKoamaru font-semibold flex justify-center items-center gap-x-2 text-sm border-deepKoamaru border-2 w-1/2"
                >
                  <BsFillBookmarkFill /> Đánh dấu
                </button>
              ) : (
                <button
                  onClick={handleCreateBookmark}
                  className="item rounded bg-deepKoamaru py-1.5 px-2 text-white font-semibold flex justify-center items-center gap-x-2 text-sm w-1/2"
                >
                  <BsFillBookmarkCheckFill /> Đã đánh dấu
                </button>
              )}

              {!upvote ? (
                <button
                  onClick={handleCreateUpvote}
                  className="item rounded bg-transparent py-1.5 px-2 text-deepKoamaru font-semibold flex justify-center items-center gap-x-2 text-sm border-deepKoamaru border-2 w-1/2"
                >
                  <BiUpArrowAlt /> Đề cử
                </button>
              ) : (
                <button
                  onClick={handleCreateUpvote}
                  className="item rounded bg-deepKoamaru py-1.5 px-2 text-white font-semibold flex justify-center items-center gap-x-2 text-sm border-deepKoamaru border-2 w-1/2"
                >
                  <BiUpArrowAlt /> Đã đề cử
                </button>
              )}
            </div>
          </div>
        </div>

        <div className="w-full mt-6 hidden flex-col md:flex">
          {getAllChaptersResponse && (
            <TabsSection
              ratingOverview={ratingOverview}
              book={book}
              getChaptersResponse={getAllChaptersResponse}
            />
          )}
        </div>

        <div className="w-full mt-6 flex flex-col md:hidden">
          {getAllChaptersResponse && (
            <MobileTabSection
              ratingOverview={ratingOverview}
              book={book}
              getChaptersResponse={getAllChaptersResponse}
            />
          )}
        </div>

        <div className="p-6">
          <CommentsSection size={7} bookId={book.id} />
        </div>

        <RatingDialog
          book={book}
          handleClose={() => setOpenRatingDialog(false)}
          open={openRatingDialog}
        />
      </div>
    </Section>
  );
};

export { BooksInfo };
