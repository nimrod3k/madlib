import enum
import random
import copy
import string
import re
import nltk
import sys


nltk.download('punkt')
nltk.download('averaged_perceptron_tagger')
#from numpy import array

posCode = {
    "CC" : "coordinating conjunction",
    "CD" : "cardinal digit",
    "DT" : "determiner",
    "EX" : "existential there",
    "FW" : "foreign word",
    "IN" : "preposition/subordinating conjunction",
    "JJ" : "adjective",
    "JJR" : "comparative adjective",
    "JJS" : "superlative adjective",
    "LS" : "list marker",
    "MD" : "modal could, will",
    "NN" : "noun",
    "NNS" : "plural noun",
    "NNP" : "singular proper noun",
    "NNPS" : "plural proper noun",
    "PDT" : "predeterminer ‘all the kids’",
    "POS" : "possessive ending",
    "PRP" :  "pronoun",
    "PRP$" : "possessive pronoun",
    "RB" : "adverb",
    "RBR" : "comparative adverb",
    "RBS" : "superlative adverb",
    "RP" : "particle give up",
    "TO" : "skip",
    "UH" : "interjection",
    "VB" : "verb",
    "VBD" : "verb (past tense)",
    "VBG" : "verb (-ing)",
    "VBN" : "verb, (past tense)",
    "VBP" : "verb",
    "VBZ" : "verb",
    "WDT" : "Question Word",
    "WP" : "Question Word",
    "WP$" : "skip",
    "WRB" : "Question Word"
    }

class MadErr(enum.Enum):
    err_none = 0
    err_unknown_exception = 1
    err_abort = 2
    err_create_illegal_tags = 3
    err_create_too_small = 4

madlib_err = {
    MadErr.err_none: "No Error", # Err_None
    MadErr.err_unknown_exception: "An Unexpected error has occurred", # Err_UnknownException
    MadErr.err_abort: "Create Madlib Aborted", # Err_Abort
    MadErr.err_create_illegal_tags: "Contains illegal Tags \"<*\" or \"*\"> Either you have already processed this into a madlib or you will need to remove the characters to proceed.", # Err_Create_IllegalTags
    MadErr.err_create_too_small: "Not enough words to make a mad lib.*-9", # Err_Create_TooSmall
    }

# Class to hold MadLib in all stages and 
class MadLib(object):
    """Main Madlib Class which holds primary functionality and storage of Madlibs"""

    # List of words to skip when creating mad-lib
    SKIP_LIST = [ "to", "too", "is", "was", "a", "of", "if", "the", "and", "but", "or", "nor", "for", "only", "in" ]
    MAX_SPACING = 7 # Default Max Spacing for mad lib creation, I might make something to save a value somewhere
    MIN_WORDS = 10

    def __init__(self):
        self.madList = list()

    # gets the ready to use MadLib story
    def get_mad_lib_text(self) -> str:
        return self._edited_text

    # Gets the error message 
    @staticmethod
    def get_error(aErr: MadErr) -> str :
        message = madlib_err.get(aErr, "Invalid Error Code Something has gone wrong")

        if message == "" :
            print(message)
        return message
        
    # Creates mad lib string 
    def create(self, base_text) :
        #try:
            if "<*" in base_text or "*>" in base_text :
                return MadErr.err_create_illegal_tags

            tokens = nltk.word_tokenize(base_text)
            pos_List = nltk.pos_tag(tokens)
            j = len(pos_List) - 1
            while j >= 0 :
                if pos_List[j][0] in string.punctuation :
                    pos_List.pop(j)
                j-=1

            # Turns the text into a string of words to process.
            the_newlines = base_text.split( '\n' )
            the_words = base_text.split()
            original_words = copy.copy(the_words)

            # Make sure there are at least 10 words. This could probably be larger.
            if len(the_words) < self.MIN_WORDS :
                return MadErr.err_create_too_small

            # Now let's pick some random spacing for word replacement
            i = 0 + random.randint(0,self.MAX_SPACING)
            while i < len(the_words) :
                # Hold any punctuation on the word
                pre = ""
                post = ""
                the_word = the_words[i]
                while the_word[0] in string.punctuation :
                    pre += the_word[0]
                    the_word = the_word[1:]
                while the_word[-1] in string.punctuation :
                    post = the_word[-1] + post
                    the_word = the_word[0:-1]

                # Skip words that don't make sense to madlib
                if the_word.lower() in self.SKIP_LIST :
                    i += random.randint(1, 3) # When Skipping don't always use the next word to make it a little more random
                    continue

                # get Context and Madlibify the word and replace it in the list
                context = self.get_context(original_words, i)
                PoS = self.get_pos(the_word, pos_List[i])
                madlibification = self.madlibify(the_word, context, PoS)

                # If we skip or have an unknown dialog return 
                if madlibification == "skip" or madlibification == the_word :
                    i += random.randint(1, 3) # When Skipping don't always use the next word to make it a little more random
                    continue
                elif madlibification == "cancel" :
                    return MadErr.err_abort

                the_words[i] = pre + madlibification + post

                i += random.randint(1, self.MAX_SPACING)

            # Re-add spaces
            _edited_text = ""
            for word in the_words :
                _edited_text += word + " "
        #except:
        #    return MadErr.err_unknown_exception
            return _edited_text
    # end of create method

    # Grabs a few words before or after the word to provide context for user to assign it's part of speech
    def get_context(self, words, word_index) -> str :
        context = ""
        minWordIndex = 0 if (word_index < 3) else word_index - 3 # Get beginning of context
        maxWordIndex = len(words) - 1 if (word_index + 3 >= len(words)) else word_index + 3 # Get end of context
        for val in  words[minWordIndex:maxWordIndex] :
            context += val + ' '
        return context
    # end of get_context method

    # Uses part of speech picker to turn the word into a MadLib tag
    def madlibify(self, madWord, context, pos = "") -> str :
        res = madWord
        #MadPicker picker = new MadPicker(madWord, context)
        #picker.ShowDialog()
        #if picker.DialogResult == DialogResult.OK:
            #res = "<*" + picker.POS + "*>";
        #else if (picker.DialogResult == DialogResult.Retry)
        #    res = "skip";
        #else if (picker.DialogResult == DialogResult.Cancel)
        #    res = "cancel";
        return res
    # end of madlibify method

    # Gets part of speech
    def get_pos(self, madWord, pos) -> str:
        ret = posCode[pos[1]]
        return ret
    # end of get_pos method

    def create_madlib(self, text = None) :
        ret = -1
        editedText = self.create(text)
        if editedText is not None :
            ret = len(self.madList)
            mad_dictionary = {
                "base_text" : text,
                "edited_text" : editedText,
                "_mad_stories" : list()
                }
            self.madList.append(mad_dictionary)
        return ret

